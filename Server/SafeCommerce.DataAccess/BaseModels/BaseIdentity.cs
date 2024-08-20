/*
 * Represents a base identity class that extends the IdentityUser.
 * This base class provides common properties to be used across various identity models in the application.
*/

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataAccess.BaseModels;

public abstract class BaseIdentity : IdentityUser
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [Required, StringLength(100)]
    public string FullName { get; set; } = null!;
    /// <summary>
    /// Gets or sets the birthday of the user.
    /// </summary>
    [Required]
    public DateTime Birthday { get; set; }
    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    [Required, Range(minimum: 18, maximum: 100)]
    public int Age { get; set; }
    /// <summary>
    /// Gets or sets the gender of the user.  
    /// </summary>
    [Required]
    public Gender Gender { get; set; }
    /// <summary>
    /// Gets or sets the is deleted of the user
    /// </summary>
    public bool IsDeleted { get; set; }
    /// <summary>
    /// Gets or sets the date created of the user
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the date modified of the user
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
    /// <summary>
    /// Gets or sets the date deleted of the user
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    /// <summary>
    /// Gets or sets the one time password of the user
    /// </summary>
    public string? OTP { get; set; }
    /// <summary>
    /// Gets or sets the one time password duration of the user
    /// </summary>
    public DateTime? OTP_Duration { get; set; }
    /// <summary>
    /// Gets or sets a flag that requires the otp during login.
    /// </summary>
    public bool RequireOTPDuringLogin { get; set; }
    /// <summary>
    /// Gets or sets the date time of last login of the user
    /// </summary>
    public DateTime? LastLogIn { get; set; }
    /// <summary>
    /// Gets or sets the date of last log out of the user
    /// </summary>
    public DateTime? LastLogOut { get; set; }
    /// <summary>
    /// Gets or sets the public key of the user
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the signature that signs the public key of the user
    /// </summary>
    public string Signature { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Signing public key of the user
    /// </summary>
    public string SigningPublicKey { get; set; } = string.Empty;
}