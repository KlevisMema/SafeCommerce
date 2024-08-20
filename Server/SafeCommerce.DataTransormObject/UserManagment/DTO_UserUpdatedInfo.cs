/* 
 * Defines a data transfer object for updated user information.
 */

using SafeCommerce.Utilities.Enums;
using SafeCommerce.DataTransormObject.Security;

namespace SafeCommerce.DataTransormObject.UserManagment;

/// <summary>
/// Represents a data transfer object for user's updated information.
/// </summary>
public class DTO_UserUpdatedInfo
{
    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    public string UserID { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the full name of the user.
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the age of the user.
    /// </summary>
    public int Age { get; set; }
    /// <summary>
    /// Gets or sets the birthday of the user.
    /// </summary>
    public DateTime Birthday { get; set; }
    /// <summary>
    /// Gets or sets the gender of the user.
    /// </summary>
    public Gender Gender { get; set; }
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the phone number of the user.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the datetime when account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the datetime when account last edited his info.
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
    /// <summary>
    /// Gets or sets the datetime when user last logged in.
    /// </summary>
    public DateTime? LastLogIn { get; set; }
    /// <summary>
    /// Gets or sets the datetime when user last logged out.
    /// </summary>
    public DateTime? LastLogOut { get; set; }
    /// <summary>
    /// Gets or sets the 2fa of the user.
    /// </summary>
    public bool RequireOTPDuringLogin { get; set; }
    /// <summary>
    /// Gets or sets the Profile Picture content of the user.
    /// </summary>
    public byte[]? ProfilePicture { get; set; }
    /// <summary>
    /// Gets or sets the Token of the user.
    /// </summary>
    public DTO_Token? UserToken { get; set; }
    /// <summary>
    /// Gets or sets the role content of the user.
    /// </summary>
    public Role Role { get; set; }
    /// <summary>
    /// Gets or sets the role content of the user.
    /// </summary>
    public string PublicKey { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the signature that signs the public key of the user
    /// </summary>
    public string Signature { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the Signing public key of the user
    /// </summary>
    public string SigningPublicKey { get; set; } = string.Empty;
}