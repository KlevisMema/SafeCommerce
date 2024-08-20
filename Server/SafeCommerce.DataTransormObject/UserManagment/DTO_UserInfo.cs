/* 
 * Defines a data transfer object for user information.
 */

using SafeCommerce.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents a data transfer object for user's essential information.
/// </summary>
public class DTO_UserInfo
{
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the birthday of the user.
    /// </summary>
    [Required]
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [Required]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    [Required]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool Enable2FA { get; set; } = false;
}