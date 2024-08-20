/* 
 * This file defines the DTO_ConfirmRegistration class in the SafeCommerce.DataTransormObject.Authentication namespace.
 * The DTO_ConfirmRegistration class is a Data Transfer Object used to encapsulate the data needed to confirm a user's registration.
 * It primarily includes a token for registration confirmation and an associated email address.
 * This class is utilized during the user registration process where confirmation through an email token is required.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Authentication;

/// <summary>
/// Data Transfer Object for confirming user registration.
/// This class is used to transfer the data required to confirm the registration of a user,
/// specifically the token and email associated with the registration process.
/// </summary>
public class DTO_ConfirmRegistration
{
    /// <summary>
    /// Gets or sets the token used for confirming registration.
    /// This token is typically sent to the user's email and is required to validate the registration.
    /// </summary>
    [Required]
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address associated with the user's account.
    /// This email is used for sending the registration confirmation token and must be a valid email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}