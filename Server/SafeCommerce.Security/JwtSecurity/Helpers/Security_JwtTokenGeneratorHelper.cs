/* 
 * Provides helper methods for generating JWT (JSON Web Token) tokens.
 * This class includes functionalities for getting signing credentials and generating tokens with specific claims.
 */

using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SafeCommerce.Security.JwtSecurity;

/// <summary>
/// Provides utility methods for generating JSON Web Tokens (JWT).
/// </summary>
internal static class Security_JwtTokenGeneratorHelper
{
    /// <summary>
    /// Gets the signing credentials for JWT token generation using the provided JWT key.
    /// </summary>
    /// <param name="jwtKey">The JWT key used for signing the token.</param>
    /// <returns>The signing credentials based on the given JWT key.</returns>
    internal static SigningCredentials
    GetSinginCredentials
    (
        string jwtKey
    )
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
    }
    /// <summary>
    /// Generates a JWT token with the specified signing credentials, claims, issuer, duration, and a flag to set default expiration behavior.
    /// </summary>
    /// <param name="singinCredentials">The signing credentials for the token.</param>
    /// <param name="claims">The list of user claims to include in the token.</param>
    /// <param name="issuer">The issuer of the token.</param>
    /// <param name="duration">The duration in minutes until the token expires.</param>
    /// <param name="defaultToken">A boolean flag indicating whether to use default expiration settings.</param>
    /// <returns>The generated JWT token.</returns>
    internal static JwtSecurityToken
    GenerateToken
    (
        SigningCredentials singinCredentials,
        List<Claim> claims,
        string issuer,
        double duration,
        bool defaultToken
    )
    {
        var token = new JwtSecurityToken
        (
            issuer: issuer,
            claims: claims,
            expires: defaultToken ? DateTime.UtcNow.AddMinutes(duration) : DateTime.UtcNow.AddMinutes(duration),
            signingCredentials: singinCredentials
        );

        return token;
    }
}