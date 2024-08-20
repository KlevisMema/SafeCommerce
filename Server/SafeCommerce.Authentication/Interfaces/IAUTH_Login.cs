/*
     * This interface provides the signature for the login 
     * functionality within the Authentication module. It ensures 
     * that any implementing class provides a method for authenticating 
     * and logging in users based on the provided DTO.
*/

using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.Authentication.Interfaces;

/// <summary>
/// Defines the contract for the login operations within the Authentication module.
/// </summary>
public interface IAUTH_Login
{
    /// <summary>
    /// Authenticates and logs in a user based on the provided login data transfer object.
    /// </summary>
    /// <param name="loginDto">The data transfer object containing user login details.</param>
    /// <returns>A generic response with a JWT token (if successful) or an error message.</returns>
    Task<Util_GenericResponse<DTO_LoginResult>>
    LoginUser
    (
        DTO_Login loginDto
    );
    /// <summary>
    /// Confirm the login of the user
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="otp">The one time password </param>
    /// <returns>A generic response with jwt token or a error message</returns>
    Task<Util_GenericResponse<DTO_LoginResult>>
    ConfirmLogin
    (
        Guid userId,
        string otp
    );
    /// <summary>
    /// Log out a user
    /// </summary>
    /// <returns> Asynchronous Task</returns>
    Task
    LogOut
    (
        string userId
    );
    /// <summary>
    /// Store users public key generated in the client
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userPublicKey">The public key of the user</param>
    /// <returns>A generic response containing the result of the operation</returns>
    Task<Util_GenericResponse<string>>
    SaveUsersPublicKey
    (
        Guid userId,
        DTO_SavePublicKey userPublicKey
    );
    /// <summary>
    ///     Validate the public key generated
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userPublicKey">The public key generated in the client</param>
    /// <returns>A generic response containing the result of the operation</returns>
    Task<Util_GenericResponse<bool>>
    VerifyPk
    (
        Guid userId,
        string userPublicKey
    );
}