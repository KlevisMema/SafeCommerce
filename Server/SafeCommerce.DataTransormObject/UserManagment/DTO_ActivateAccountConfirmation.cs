/* 
 * This file defines the DTO_ActivateAccountConfirmation class in the SafeCommerce.DataTransormObject.UserManagment namespace.
 * The DTO_ActivateAccountConfirmation class is a Data Transfer Object used to encapsulate information required for confirming the activation of a user account,
 * including the user's email and the activation token.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents the information required for confirming the activation of a user account.
/// This class includes the user's email and the unique activation token.
/// </summary>
public class DTO_ActivateAccountConfirmation
{
    /// <summary>
    /// Gets or sets the email address associated with the user account to be activated. 
    /// This property is required and should be a valid email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the activation token for the user account. 
    /// This property is required and is used to verify the validity of the account activation request.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
}