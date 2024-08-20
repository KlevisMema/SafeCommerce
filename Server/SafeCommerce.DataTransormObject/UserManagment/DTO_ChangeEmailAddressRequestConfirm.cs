/*
 * This file defines the DTO_ChangeEmailAddressRequestConfirm class in the SafeCommerce.DataTransormObject.UserManagment namespace.
 * The DTO_ChangeEmailAddressRequestConfirm class is a Data Transfer Object used to encapsulate the necessary information for confirming a request to change a user's email address,
 * including the email address and the confirmation token.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents the information required to confirm a change of email address request.
/// This class includes the new email address and the token associated with the request.
/// </summary>
public class DTO_ChangeEmailAddressRequestConfirm
{
    /// <summary>
    /// Gets or sets the new email address that was requested for the change.
    /// This property is required and should be in a valid email address format.
    /// </summary>
    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the token associated with the email address change request.
    /// This property is required and should contain the valid token for confirming the change.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
}