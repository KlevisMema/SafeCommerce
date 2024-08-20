using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.IP;
using SafeCommerce.Utilities.Log;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using Microsoft.Extensions.Configuration;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.Security.JwtSecurity.Interfaces;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.Security.JwtSecurity.Implementations;

namespace SafeCommerce.Authentication.Auth;

/// <summary>
/// IInitializes a new instance of the <see cref="AUTH_RefreshToken"/> class.
/// </summary>
/// <param name="logger">The logger used for logging</param>
/// <param name="db">The instance of <see cref="ApplicationDbContext"/></param>
/// <param name="mapper">The auto mapper instance <see cref="IMapper"/> used for mappings</param>
/// <param name="userManager">The user manager instance <see cref="UserManager{TUser}"/></param>
/// <param name="configuration">The configurations settings instance <see cref="IConfiguration"/></param>
/// <param name="tokenValidationParameters">An instance of <see cref="TokenValidationParameters"/></param>
/// <param name="securityJwtTokenHash">The jwt hash token instance <see cref="ISecurity_JwtTokenHash"/></param>
/// <param name="httpContextAccessor"> The http context accessor instance <see cref="IHttpContextAccessor"/> used to get user ip address</param>
/// <param name="jwtTokenService">The jwt token instace service <see cref="ISecurity_JwtTokenAuth{TService, TFuncInputParamType, TReturnType}"/></param>
public class AUTH_RefreshToken
(
    IMapper mapper,
    ApplicationDbContext db,
    IConfiguration configuration,
    ILogger<AUTH_RefreshToken> logger,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor,
    ISecurity_JwtTokenHash securityJwtTokenHash,
    TokenValidationParameters tokenValidationParameters,
    ISecurity_JwtTokenAuth<Security_JwtTokenAuth, DTO_AuthUser, DTO_Token> jwtTokenService
) : Util_BaseAuthDependencies<AUTH_RefreshToken, ApplicationUser, ApplicationDbContext>(
    mapper,
    logger,
    httpContextAccessor,
    userManager,
    configuration,
    db
), IAUTH_RefreshToken
{
    /// <summary>
    /// The <see cref="ISecurity_JwtTokenHash"/> used to validate a token
    /// </summary>
    private readonly ISecurity_JwtTokenHash _securityJwtTokenHash = securityJwtTokenHash;
    /// <summary>
    /// The <see cref="TokenValidationParameters"/>
    /// </summary>
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    /// <summary>
    /// The <see cref="ISecurity_JwtTokenAuth{TService, TFuncInputParamType, TReturnType}"/>
    /// used for jwt token operations such as generating a new token.
    /// </summary>
    private readonly ISecurity_JwtTokenAuth<Security_JwtTokenAuth, DTO_AuthUser, DTO_Token> _jwtTokenService = jwtTokenService;

    /// <summary>
    /// Refreshes a token of a expired jwt token
    /// </summary>
    /// <param name="validateTokenDto">The <see cref="DTO_ValidateToken"/> object</param>
    /// <returns> A generic response indicating if the operation ended successfully with the new token or not </returns>
    public async Task<Util_GenericResponse<DTO_Token>>
    RefreshToken
    (
        DTO_ValidateToken validateTokenDto
    )
    {
        try
        {
            var claimsPrincipal = GetClaimsPrincipal(validateTokenDto.Token);

            if (claimsPrincipal is null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP} user has no claims in his token.
                        Invalid token. Token {Token}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    validateTokenDto.Token
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Invalid token",
                    null,
                    System.Net.HttpStatusCode.Unauthorized
                );
            }

            if (claimsPrincipal.Claims is null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. Refresh token with [ID] {refreshToken} doesn't have 
                        any claims.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    validateTokenDto.RefreshTokenId
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Could not find any claims",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            string? expiryDate = claimsPrincipal.Claims?.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

            if (expiryDate == null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. Refresh token with [ID] {refreshToken} doesn't have an expiry date 
                        in the claims principal.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    validateTokenDto.RefreshTokenId
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Could not find a expiry date in claims",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            var expiryDateUnix = long.Parse(expiryDate);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. Token has not expired yet.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor)
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    true,
                    "Token has not expired yet",
                    null,
                    System.Net.HttpStatusCode.OK
                );
            }

            string? jti = claimsPrincipal.Claims?.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (jti == null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. The jti could not be found in claims {@Claims}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    claimsPrincipal.Claims
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                   null,
                   false,
                   "Could not find the jti",
                   null,
                   System.Net.HttpStatusCode.NotFound
                );
            }

            var storedRefreshToken = await _db.RefreshTokens.Include(x => x.User)
                                                            .SingleOrDefaultAsync(x => x.Id == validateTokenDto.RefreshTokenId);

            if (storedRefreshToken is null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. Refresh token with [ID] {refreshToken} doesn't  exists in database.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    validateTokenDto.RefreshTokenId
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Refresh token does not exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            if (!await _securityJwtTokenHash.ValidateToken(validateTokenDto.RefreshToken, storedRefreshToken.HashedToken))
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. Refresh token doesn't {refreshToken} is not a valid refresh token.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    validateTokenDto.RefreshToken
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Invalid Refresh token",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. User with [ID] {userId}, Refresh token {refreshToken} has expired at {expiryDate}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    storedRefreshToken.UserId,
                    storedRefreshToken.HashedToken,
                    storedRefreshToken.ExpiryDate
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Refresh token has expired",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            if (storedRefreshToken.Invaidated)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. User with [ID] {userId}, Refresh token {refreshToken} has been invalidated so it's not usable.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    storedRefreshToken.UserId,
                    storedRefreshToken.HashedToken
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Refresh token has been invalidated",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            if (storedRefreshToken.Used)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. User with [ID] {userId}, Refresh token {refreshToken} 
                        has already been used.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    storedRefreshToken.UserId,
                    storedRefreshToken.HashedToken
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Refresh token has been used",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            if (storedRefreshToken.JwtId != jti)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                        [RESULT] : [IP] {IP}. User with [ID] {userId}, Refresh token {refreshToken} 
                        doesn't match the jti. The provided jti by the user {userProvidedJti} |
                        stored jti {storedJti}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    storedRefreshToken.UserId,
                    storedRefreshToken.HashedToken,
                    jti,
                    storedRefreshToken.JwtId
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Refresh token does not match the jti",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            storedRefreshToken.Used = true;

            _db.RefreshTokens.Update(storedRefreshToken);
            await _db.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(storedRefreshToken.User);
            var userDto = _mapper.Map<DTO_AuthUser>(storedRefreshToken.User);
            userDto.Roles = [.. roles];
            var token = await _jwtTokenService.CreateToken(userDto);

            _logger.Log
            (
                LogLevel.Information,
                """
                    [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method] => 
                    [RESULT] : [IP] {IP}. User with [ID] {userId}, a new token was issued at {dateTokenIssued}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                storedRefreshToken.UserId,
                token.CreatedAt
            );

            return Util_GenericResponse<DTO_Token>.Response
            (
                token,
                true,
                "Refresh token issued successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_Token, AUTH_RefreshToken>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [Authentication Module]-[AUTH_RefreshToken Class]-[RefreshToken Method].
                 """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Get claims for the provided token
    /// </summary>
    /// <param name="token">The token</param>
    /// <returns> <see cref="ClaimsPrincipal"/> </returns>
    private ClaimsPrincipal?
    GetClaimsPrincipal
    (
        string token
    )
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                return null;

            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            // this is a case when the the token has expired which will make validatedToken null
            // but we want to extract claims even if the token life time has expired.
            if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken) return null;

            var claimsIdentity = new ClaimsIdentity(jwtToken.Claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// Checks if the validated token by life time is 
    /// generated with the appropriate security algorithm
    /// </summary>
    /// <param name="validatedToken"> The validated token by lifetime </param>
    /// <returns> True or false </returns>
    private static bool
    IsJwtWithValidSecurityAlgorithm
    (
        SecurityToken validatedToken
    )
    {
        return (validatedToken is JwtSecurityToken jwtSecurity) &&
                jwtSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase);
    }
}