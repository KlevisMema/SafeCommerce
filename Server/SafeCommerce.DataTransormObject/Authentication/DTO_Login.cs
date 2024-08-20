/*
 * Defines the data transfer object for user login.
 * This DTO captures the necessary fields required for a user to log into the system.
*/

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Authentication;

/// <summary>
/// A DTO that represents the login fields.
/// </summary>
public class DTO_Login
{
    /// <summary>
    /// Gets or sets the email of the use logging in.
    /// </summary>
    [Required(ErrorMessage = "Email field is required ")]
    [EmailAddress(ErrorMessage = "Email is not a vaild email format")]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the password of the user loging in.
    /// </summary>
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password field is required")]
    [StringLength(maximumLength: 30, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = string.Empty;
}