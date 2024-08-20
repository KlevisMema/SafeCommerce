/* 
 * This interface defines the contract for account management operations,
 * including retrieving user information, updating user details, 
 * deleting users, and changing user passwords.
*/

using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.UserManagment.Interfaces;

/// <summary>
/// Provides an interface for account management operations.
/// </summary>
public interface IAccountManagment
{
    /// <summary>
    /// Fetches user information based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>A generic response containing the user's updated information.</returns>
    Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    GetUser
    (
        Guid id
    );
    /// <summary>
    /// Updates the user's information based on the provided user ID and user data.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="dtoUser">The user's new information.</param>
    /// <returns>A generic response containing the user's updated information after the changes.</returns>
    Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    UpdateUser
    (
        Guid id,
        DTO_UserInfo dtoUser
    );
    /// <summary>
    /// Deletes the user based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>A generic response indicating whether the user was successfully deleted or not.</returns>
    Task<Util_GenericResponse<bool>>
    DeactivateAccount
    (
        Guid id,
         DTO_DeactivateAccount deactivateAccount
    );
    /// <summary>
    /// Changes the password of a user based on the provided user ID and password details.
    /// </summary>
    /// <param name="id">The ID of the user whose password needs to be changed.</param>
    /// <param name="updatePassword">The details containing the old and new password information.</param>
    /// <returns>A generic response indicating whether the password was successfully changed or not.</returns>
    Task<Util_GenericResponse<bool>>
    ChangePassword
    (
        Guid id,
        DTO_UserChangePassword updatePassword
    );
    /// <summary>
    /// Send an email to the user email with the link to reset his password.
    /// </summary>
    /// <param name="email"> The email of the user </param>
    /// <returns> A generic result indicating the result of the operation </returns>
    Task<Util_GenericResponse<bool>>
    ForgotPassword
    (
        string email
    );
    /// <summary>
    /// Reset the password of the user.
    /// </summary>
    /// <param name="resetPassword">The reset password object</param>
    /// <returns>A generic response indicating if the result of the operation</returns>
    Task<Util_GenericResponse<bool>>
    ResetPassword
    (
        DTO_ResetPassword resetPassword
    );
    /// <summary>
    /// Reactivate the account request
    /// </summary>
    /// <param name="email"> The email of the user </param>
    /// <returns> A generic response indicating whether the user activation request was successfully or not </returns>
    Task<Util_GenericResponse<bool>>
    ActivateAccountRequest
    (
        string email
    );
    /// <summary>
    /// Confirms the account activation
    /// </summary>
    /// <param name="accountConfirmation"> The <see cref=DTO_ActivateAccountConfirmation""/> object </param>
    /// <returns> A generic response indicating if the users account was activated or not </returns>
    Task<Util_GenericResponse<bool>>
    ActivateAccountConfirmation
    (
        DTO_ActivateAccountConfirmation accountConfirmation
    );
    /// <summary>
    /// Request for changing the email
    /// </summary>
    /// <param name="newEmailAddressDto">The <see cref="DTO_ChangeEmailAddressRequest"/> object dto </param>
    /// <returns>A generic response indicating the result of the operation</returns>
    Task<Util_GenericResponse<bool>>
    RequestChangeEmailAddress
    (
        Guid userId,
       DTO_ChangeEmailAddressRequest newEmailAddressDto
    );
    /// <summary>
    /// Confirms the request of changing the email.
    /// </summary>
    /// <param name="changeEmailAddressConfirmDto"> The <see cref="DTO_ChangeEmailAddressRequestConfirm"/> object dto </param>
    /// <returns>A generic response indicating the result of the operation</returns>
    Task<Util_GenericResponse<DTO_Token>>
    ConfirmChangeEmailAddressRequest
    (
        Guid userId,
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    );
    /// <summary>
    /// Search usesr by their username
    /// </summary>
    /// <param name="userName">The username of the user</param>
    /// <param name="userId">The id of the user</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns>A generic response indicating the result of the operation</returns>
    Task<Util_GenericResponse<List<DTO_UserSearched>>>
    SearchUserByUserName
    (
        string userName,
        string userId
    );
    /// <summary>
    /// Uploads an image for a user
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="image"> The image of the user</param>
    /// <returns>A generic response indicating the result of the operation</returns>
    Task<Util_GenericResponse<byte[]>>
    UploadProfilePicture
    (
        Guid userId,
        IFormFile? image
    );
}