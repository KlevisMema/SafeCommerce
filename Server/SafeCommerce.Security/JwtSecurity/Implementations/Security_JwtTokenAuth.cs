/* 
 * Provides implementation for the ISecurity_JwtTokenAuth interface, responsible for JWT token generation.
 * This class offers methods to create JWT tokens for user authentication, utilizing JWT settings provided through configuration.
 * It also includes functionality for validating JWT tokens and adding them to the database.
 */

using System.Security.Claims;
using Microsoft.Extensions.Options;
using SafeCommerce.DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Security.JwtSecurity.Helpers;
using SafeCommerce.Security.JwtSecurity.Interfaces;
using SafeCommerce.Utilities.ConfigurationSettings;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.DataTransormObject.Security;

namespace SafeCommerce.Security.JwtSecurity.Implementations;

/// <summary>
/// Service responsible for creating and validating JWT tokens for authentication.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OAuthJwtTokenService"/> class.
/// </remarks>
/// <param name="db">The database context</param>
/// <param name="jwtOptions">The JWT authentication options.</param>
public class Security_JwtTokenAuth
(
    ApplicationDbContext db,
    IOptions<Util_JwtSettings> jwtOptions
) : ISecurity_JwtTokenAuth<Security_JwtTokenAuth, DTO_AuthUser, DTO_Token>, ISecurity_JwtTokenHash
{
    /// <summary>
    /// Creates a JWT token based on the specified input parameter.
    /// </summary>
    /// <param name="user">The input parameter used for token generation. This could be user information or any other required data.</param>
    /// <returns>A task representing the asynchronous operation. The task result is of type <see cref="DTO_Token"/>, representing the generated JWT token.</returns>
    public async Task<DTO_Token>
    CreateToken
    (
        DTO_AuthUser user
    )
    {
        var singinCredentials = Security_JwtTokenGeneratorHelper.GetSinginCredentials(jwtOptions.Value.Key);
        var claims = GetClaims(user);
        var token = Security_JwtTokenGeneratorHelper.GenerateToken(singinCredentials, claims, jwtOptions.Value.Issuer, Convert.ToDouble(jwtOptions.Value.LifeTime), true);

        var tokenDto = await AddToDb(token.Id, claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
        tokenDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
        tokenDto.ValididtyTime = token.ValidTo;
        return tokenDto;
    }
    /// <summary>
    /// Validates a JWT token against a hashed version stored in the database.
    /// </summary>
    /// <param name="tokenToValidate">The JWT token to validate.</param>
    /// <param name="hashedTokenInDb">The hashed version of the token stored in the database.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating the validity of the token.</returns>
    public Task<bool>
    ValidateToken
    (
        string tokenToValidate,
        string hashedTokenInDb
    )
    {
        byte[] key = GetKeyFromBase64String();

        var hmacHelper = new Security_HMACSHA256Helper(key);

        string hashedTokenToValidate = hmacHelper.ComputeHash(tokenToValidate);

        return Task.FromResult(hashedTokenInDb == hashedTokenToValidate);
    }
    /// <summary>
    /// Add the refresh token in the database
    /// </summary>
    /// <param name="tokenId">The id of the token</param>
    /// <param name="userId">The id of the user</param>
    /// <returns>A <see cref="DTO_Token"/> object </returns>
    private async Task<DTO_Token>
    AddToDb
    (
        string tokenId,
        string userId
    )
    {
        try
        {
            var randomId = Guid.NewGuid().ToString();

            byte[] key = GetKeyFromBase64String();
            
            var hmacHelper = new Security_HMACSHA256Helper(key);
            var hashedRefreshToken = hmacHelper.ComputeHash(randomId);

            var refreshToken = new RefreshToken
            {
                HashedToken = hashedRefreshToken,
                JwtId = tokenId,
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
            };

            await db.RefreshTokens.AddAsync(refreshToken);

            await db.SaveChangesAsync();

            return new DTO_Token { RefreshToken = randomId, RefreshTokenId = refreshToken.Id, CreatedAt = refreshToken.CreationDate };
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    /// <summary>
    /// Gets the claims for the specified user.
    /// </summary>
    /// <param name="user">The user information.</param>
    /// <returns>The list of claims.</returns>
    private List<Claim>
    GetClaims
    (
        DTO_AuthUser user
    )
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.NameId, user.Id!),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Aud, jwtOptions.Value.Audience),
            new(JwtRegisteredClaimNames.Iss, jwtOptions.Value.Issuer),
            new(JwtRegisteredClaimNames.FamilyName, user.FullName),
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private static byte[] 
    GetKeyFromBase64String
    ()
    {
        string base64Key = Environment.GetEnvironmentVariable("SAFE_SHARE_HMAC");
        return Convert.FromBase64String(base64Key);
    }
}