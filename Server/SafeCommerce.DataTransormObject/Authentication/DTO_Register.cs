using SafeCommerce.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

/*
 * Defines the data transfer object for user registration.
 * This DTO captures the necessary fields required to register a new user in the system.
*/

namespace SafeCommerce.DataTransormObject.Authentication;

/// <summary>
/// A DTO that represents the registering fields of the user.
/// </summary>
public class DTO_Register
{
    /// <summary>
    /// Gets or sets the user name of the user registering.
    /// </summary>
    [Required, StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full name of the user registering.
    /// </summary>
    [Required(ErrorMessage = "Full name is required"), StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the birthday of the user registering.
    /// </summary>
    [Required]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Gets or sets the phone number of user registering.
    /// </summary>
    [Required]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the gender of the user registering.  
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the email of the user registering.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///  Gets or sets the password of the user registering.
    /// </summary>
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///  Gets or sets the confirm password of the user registering.
    /// </summary>
    [Required(ErrorMessage = "Confirm password is required"), DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the 2 factor auth of the user registering.
    /// </summary>
    public bool Enable2FA { get; set; } = false;
}