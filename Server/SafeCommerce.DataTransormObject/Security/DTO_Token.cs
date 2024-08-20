/* 
 * This file defines the DTO_Token class in the SafeCommerce.DataTransormObject.Security namespace.
 * The DTO_Token class is a Data Transfer Object used to encapsulate token-related information,
 * including the authentication token, refresh token, the identifier of the refresh token, and the token's creation time.
 */

namespace SafeCommerce.DataTransormObject.Security;

/// <summary>
/// Represents token-related information used in the security layer of the application.
/// This class includes the authentication token, refresh token, and their associated metadata.
/// </summary>
public class DTO_Token
{
    /// <summary>
    /// Gets or sets the JWT authentication token. This property is nullable and may be empty.
    /// </summary>
    public string? Token { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the refresh token used to obtain a new authentication token when the current one expires.
    /// This property is nullable and may be empty.
    /// </summary>
    public string? RefreshToken { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the unique identifier of the refresh token.
    /// </summary>
    public Guid RefreshTokenId { get; set; }
    /// <summary>
    /// Gets or sets the creation time of the token. This property is nullable.
    /// </summary>
    public DateTime? CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the validity of the time for the token
    /// </summary>
    public DateTime ValididtyTime { get; set; }
}