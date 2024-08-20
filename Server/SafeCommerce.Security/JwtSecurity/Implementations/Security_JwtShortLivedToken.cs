/* 
 * Implements the functionality to create short-lived JWT (JSON Web Tokens) for authentication purposes.
 * This class provides methods to generate tokens with a limited lifespan, typically used for operations like one-time password (OTP) authentication.
 */

using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using SafeCommerce.Security.JwtSecurity.Interfaces;
using SafeCommerce.Utilities.ConfigurationSettings;

namespace SafeCommerce.Security.JwtSecurity.Implementations;

/// <summary>
/// Implements the generation of short-lived JWT tokens for specific authentication scenarios.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Security_JwtShortLivedToken"/> class.
/// </remarks>
/// <param name="configuration">The application configuration to access settings like OTP duration.</param>
/// <param name="jwtOptions">The JWT settings to be used for token generation.</param>
public class Security_JwtShortLivedToken
(
    IConfiguration configuration,
    IOptions<Util_JwtSettings> jwtOptions
) : ISecurity_JwtTokenAuth<Security_JwtShortLivedToken, string, string>
{
    /// <summary>
    /// Creates a short-lived JWT token for the specified user.
    /// </summary>
    /// <param name="userId">The user identifier for whom the token is being created.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the generated JWT token string.</returns>
    public Task<string>
    CreateToken
    (
        string userId
    )
    {
        var singinCredentials = Security_JwtTokenGeneratorHelper.GetSinginCredentials(jwtOptions.Value.KeyConfrimLogin);
        var claims = GetClaims(userId);
        var token = Security_JwtTokenGeneratorHelper.GenerateToken(singinCredentials, claims, jwtOptions.Value.Issuer, Convert.ToDouble(configuration.GetSection("OTP_Duration").Value), false);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
    /// <summary>
    /// Gets the list of claims to be included in the JWT token.
    /// </summary>
    /// <param name="userId">The user identifier whose claims are being prepared.</param>
    /// <returns>A list of claims for the specified user.</returns>
    private List<Claim>
    GetClaims
    (
        string userId
    )
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Aud, jwtOptions.Value.Audience),
            new Claim(JwtRegisteredClaimNames.Iss, jwtOptions.Value.Issuer)
        };

        return claims;
    }
}