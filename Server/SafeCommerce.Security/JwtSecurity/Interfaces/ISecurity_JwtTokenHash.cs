/* 
 * Defines the interface ISecurity_JwtTokenHash responsible for validating JWT (JSON Web Tokens) tokens.
 * This interface provides a method for validating tokens against a hashed token stored in the database, 
 * ensuring the authenticity and integrity of the token for user authentication processes.
 */

namespace SafeCommerce.Security.JwtSecurity.Interfaces;

/// <summary>
/// Interface for services that validate JWT (JSON Web Tokens) tokens.
/// Provides a method for validating the authenticity and integrity of JWT tokens.
/// </summary>
public interface ISecurity_JwtTokenHash
{
    /// <summary>
    /// Validates a JWT token against a hashed version stored in the database.
    /// </summary>
    /// <param name="tokenToValidate">The JWT token to validate.</param>
    /// <param name="hashedTokenInDb">The hashed version of the token stored in the database.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating the validity of the token.</returns>
    Task<bool>
    ValidateToken
    (
       string tokenToValidate,
       string hashedTokenInDb
    );
}