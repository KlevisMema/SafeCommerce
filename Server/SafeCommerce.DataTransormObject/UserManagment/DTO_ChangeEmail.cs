/* 
 * This file defines the DTO_ChangeEmail class in the SafeCommerce.DataTransormObject.UserManagment namespace.
 * The DTO_ChangeEmail class is a Data Transfer Object used to encapsulate the necessary information for changing a user's email address,
 * including the new email address and its confirmation.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents the information required for changing a user's email address.
/// This class includes the new email address and a confirmation of the new email address.
/// </summary>
public class DTO_ChangeEmail
{
    /// <summary>
    /// Gets or sets the new email address for the user. 
    /// This property is required and should be a valid email address format.
    /// </summary>
    [Required, EmailAddress]
    public string NewEmail { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the confirmation of the new email address. 
    /// This property is required and should match the new email address and be in a valid email address format.
    /// </summary>
    [Required, EmailAddress]
    public string ConfirmNewEmail { get; set; } = string.Empty;
}