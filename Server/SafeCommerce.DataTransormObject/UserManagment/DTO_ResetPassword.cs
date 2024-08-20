/* This file defines the DTO_ResetPassword class in the SafeCommerce.DataTransormObject.UserManagment namespace.
* The DTO_ResetPassword class is a Data Transfer Object used to encapsulate the necessary information for resetting a user's password.
*/

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents a Data Transfer Object (DTO) used for resetting a user's password.
/// </summary>
public class DTO_ResetPassword
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the token used for password reset.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the new password for the user.
    /// </summary>
    [Required, DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the confirmation of the new password.
    /// </summary>
    [Required, DataType(DataType.Password), Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}