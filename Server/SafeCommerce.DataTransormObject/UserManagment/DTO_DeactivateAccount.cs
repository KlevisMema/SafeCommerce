/* 
 * This file defines the DTO_DeactivateAccount class in the SafeCommerce.DataTransormObject.UserManagment namespace.
 * The DTO_DeactivateAccount class is a Data Transfer Object used to encapsulate the necessary information for deactivating a user's account.
 */

using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// This class defines the DTO_DeactivateAccount class in the SafeCommerce.DataTransormObject.UserManagment namespace.
/// The DTO_DeactivateAccount class is a Data Transfer Object used to encapsulate the necessary information for deactivating a user's account,
/// including the user's email address and password.
/// </summary>
public class DTO_DeactivateAccount
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}