/* 
 * Defines the contract for a service responsible for refreshing JWT (JSON Web Tokens).
 * This interface provides a method for refreshing tokens based on specific token validation information.
 */

using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.Security.JwtSecurity.Interfaces;

/// <summary>
/// Defines an interface for a service that handles the refreshing of JWT (JSON Web Tokens).
/// </summary>
public interface ISecurity_RefreshToken
{
    /// <summary>
    /// Refreshes a JWT token based on the provided token validation data.
    /// </summary>
    /// <param name="validateTokenDto">Data transfer object containing information required for validating and refreshing the token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a generic response with the new token details.</returns>
    Task<Util_GenericResponse<DTO_Token>>
    RefreshToken
    (
        DTO_ValidateToken validateTokenDto
    );
}