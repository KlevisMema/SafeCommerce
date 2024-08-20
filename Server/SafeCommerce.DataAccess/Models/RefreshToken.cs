/*
 * This file defines the RefreshToken class in the SafeCommerce.DataAccessLayer.Models namespace.
 * The RefreshToken class represents the model for storing refresh tokens in the database.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccess.Models;
public class RefreshToken
{
    /// <summary>
    /// Gets or sets the unique identifier for the refresh token.
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the hashed value of the refresh token.
    /// </summary>
    [Required]
    [StringLength(64)]
    public string HashedToken { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the unique identifier for the associated JWT.
    /// </summary>
    public string JwtId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the creation date of the refresh token.
    /// </summary>
    public DateTime? CreationDate { get; set; }
    /// <summary>
    /// Gets or sets the expiry date of the refresh token.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    /// <summary>
    /// Gets or sets a flag indicating whether the refresh token has been used.
    /// </summary>
    public bool Used { get; set; }
    /// <summary>
    /// Gets or sets a flag indicating whether the refresh token is invalidated.
    /// </summary>
    public bool Invaidated { get; set; }
    /// <summary>
    /// Gets or sets the user ID associated with the refresh token.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the user associated with the refresh token.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;
}