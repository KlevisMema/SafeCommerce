/* 
 * Defines a data transfer object for changing user passwords.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents a data transfer object for users to change their password.
/// </summary>
public class DTO_UserChangePassword
{
    /// <summary>
    /// Gets or sets the old password.
    /// </summary>
    [Required(ErrorMessage = "Old password is required"), DataType(DataType.Password)]
    public string OldPassword { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    [Required(ErrorMessage = "New password is required"), DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the confirmation for the new password.
    /// </summary>
    [Required(ErrorMessage = "Confirm new password is required"), DataType(DataType.Password), Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}