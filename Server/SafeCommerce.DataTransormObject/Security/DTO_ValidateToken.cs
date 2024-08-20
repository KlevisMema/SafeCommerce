/* 
 * This file defines the DTO_ValidateToken class in the SafeCommerce.DataTransormObject.Security namespace.
 * The DTO_ValidateToken class is a Data Transfer Object used to encapsulate the necessary information for validating an authentication token,
 * including the token itself, the associated refresh token, and the unique identifier of the refresh token.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Security;

/// <summary>
/// Represents the necessary information required for validating an authentication token.
/// This class includes the token, refresh token, and the unique identifier of the refresh token.
/// </summary>
public class DTO_ValidateToken
{
    /// <summary>
    /// Gets or sets the JWT authentication token that needs to be validated.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the refresh token associated with the authentication token.
    /// </summary>
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the unique identifier of the refresh token.
    /// </summary>
    [Required]
    public Guid RefreshTokenId { get; set; }
}