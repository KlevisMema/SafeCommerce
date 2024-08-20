/* 
 * This file defines the DTO_ChangeEmailAddressRequest class in the SafeCommerce.DataTransormObject.UserManagment namespace.
 * The DTO_ChangeEmailAddressRequest class is a Data Transfer Object used to encapsulate the necessary information for requesting a change of a user's email address,
 * including the current email address, the new email address, and its confirmation.
 */

using System.ComponentModel.DataAnnotations;
using SafeCommerce.DataTransormObject.Validators;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents the information required for requesting a change of a user's email address.
/// This class includes the current email address, the new email address, and its confirmation.
/// </summary>
public class DTO_ChangeEmailAddressRequest
{
    /// <summary>
    /// Gets or sets the current email address of the user.
    /// This property is required and should be in a valid email address format.
    /// </summary>
    [Required, EmailAddress]
    public string CurrentEmailAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the new email address for the user. 
    /// This property is required and should be in a valid email address format.
    /// It must not be equal to the current email address.
    /// </summary>
    [Required, EmailAddress, IsNotEqualTo("CurrentEmailAddress")]
    public string NewEmailAddress { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the confirmation of the new email address. 
    /// This property is required and should match the new email address and be in a valid email address format.
    /// It must not be equal to the current email address.
    /// </summary>
    [Required, EmailAddress, IsNotEqualTo("CurrentEmailAddress")]
    public string ConfirmNewEmailAddress { get; set; } = string.Empty;
}