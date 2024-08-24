/*
 * Account Management Class
 * 
 * This class encapsulates all the operations related to account management within the SafeShare application.
 * It provides methods for fetching user details, updating user information, deleting accounts, and changing user passwords.
 * Additionally, it utilizes the services provided by UserManager and the application's database context for various operations.
*/

using AutoMapper;
using SafeCommerce.Utilities.IP;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using Microsoft.Extensions.Options;
using SafeCommerce.Utilities.Email;
using Microsoft.Extensions.Logging;
using SafeCommerce.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.Security.JwtSecurity.Interfaces;
using SafeCommerce.Utilities.ConfigurationSettings;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.Security.JwtSecurity.Implementations;

namespace SafeCommerce.UserManagment.UserAccount;

/// <summary>
/// This class encapsulates all the operations related to account management within the SafeShare application
/// </summary>
/// <remarks>
/// Initializes a new instance of the AccountManagment class.
/// </remarks>
/// <param name="resetPasswordSettings">Reset password settings</param>
/// <param name="logger">Logger instance for logging operations.</param>
/// <param name="db">The application's database context instance.</param>
/// <param name="changeEmailAddressSettings">Email change settings</param>
/// <param name="activateAccountSettings">Activate account settings</param>
/// <param name="mapper">The AutoMapper instance used for object-object mapping.</param>
/// <param name="httpContextAccessor">Provides information about the HTTP request.</param>
/// <param name="jwtTokenService">Provides services to handle jwt token operations.</param>
/// <param name="userManager">UserManager instance to manage users in the persistence store.</param>
public class AccountManagment
(
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<AccountManagment> logger,
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IOptions<Util_ResetPasswordSettings> resetPasswordSettings,
    IOptions<Util_ActivateAccountSettings> activateAccountSettings,
    IOptions<Util_ChangeEmailAddressSettings> changeEmailAddressSettings,
    ISecurity_JwtTokenAuth<Security_JwtTokenAuth, DTO_AuthUser, DTO_Token> jwtTokenService
) : Util_BaseContextDependencies<ApplicationDbContext, AccountManagment>
(
    db,
    mapper,
    logger,
    httpContextAccessor
), IAccountManagment
{
    /// <summary>
    /// Retrieves the user details based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to be retrieved.</param>
    /// <returns>A response containing user details.</returns>
    public async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    GetUser
    (
        Guid id
    )
    {
        var getUserResult = await GetUserInfoMapped("retrieved", id, null);

        return getUserResult;
    }
    /// <summary>
    /// Updates user details based on the provided user ID and updated user information.
    /// </summary>
    /// <param name="id">The ID of the user to be updated.</param>
    /// <param name="dtoUser">The updated user information.</param>
    /// <returns>A response indicating the result of the update operation.</returns>
    public async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    UpdateUser
    (
        Guid id,
        DTO_UserInfo dtoUser
    )
    {
        try
        {
            var getUser = await GetApplicationUser(id);

            if (getUser is null || getUser.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[UpdateUser Method] =>
                        [IP] {IP}
                        User with [ID] {id} doesn't exists
                        Dto {@DTO} and User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    dtoUser,
                    getUser
                );

                return Util_GenericResponse<DTO_UserUpdatedInfo>.Response
                (
                    null,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            getUser.ModifiedAt = DateTime.UtcNow;
            getUser.FullName = dtoUser.FullName;
            getUser.Birthday = dtoUser.Birthday;
            getUser.Gender = dtoUser.Gender;
            getUser.UserName = dtoUser.UserName;
            getUser.NormalizedUserName = dtoUser.UserName.ToUpper();
            getUser.PhoneNumber = dtoUser.PhoneNumber;
            getUser.RequireOTPDuringLogin = dtoUser.Enable2FA;
            getUser.Age = DateTime.UtcNow.Year - dtoUser.Birthday.Year;

            await DeleteUserRefreshTokens(getUser.Id);

            var token = await GetToken(getUser);

            _logger.Log
            (
                LogLevel.Information,
                """
                     [UserManagment Module]-[AccountManagment Class]-[UpdateUser Method],
                     [IP] {IP}
                     User with [ID] : {id} just updated his data at {userModifiedAt}
                     Dto {@DTO} | User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                id,
                getUser.ModifiedAt,
                dtoUser,
                getUser
            );

            return await GetUserInfoMapped("updated", id, token);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.Log
            (
                LogLevel.Critical,
                """
                        [UserManagment Module]-[AccountManagment Class]-[UpdateUser Method],
                        [IP] {IP} User with [ID] : {id} tried to update his data and got the error
                        {ex}
                    """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                id,
                dbEx
            );

            return Util_GenericResponse<DTO_UserUpdatedInfo>.Response
            (
                null,
                false,
                "Something went wrong, username was not updated",
                null,
                System.Net.HttpStatusCode.BadRequest
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_UserUpdatedInfo, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                     Something went wrong in [UserManagment Module]-[AccountManagment Class]-[UpdateUser Method],
                     user with [ID] {id}.
                 """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Deactivates the user based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to deactivate.</param>
    /// <returns>A generic response indicating whether the user was successfully deactivated or not.</returns>
    public async Task<Util_GenericResponse<bool>>
    DeactivateAccount
    (
        Guid id,
        DTO_DeactivateAccount deactivateAccount
    )
    {
        try
        {

            var user = await GetApplicationUser(id);

            if (user is null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[DeactivateAccount Method] =>
                         [IP] {IP}
                         User with [ID] {id} doesn't exists.
                         DTO {@DTO} | User {@User} .
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    deactivateAccount,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            var identifyDeletionIsByTheUser = await userManager.CheckPasswordAsync(user, deactivateAccount.Password);

            if (!identifyDeletionIsByTheUser)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[DeactivateAccount Method] =>
                        User with [IP] {IP}
                        tried to delete user with [ID] {ID} doesn't exists
                        DTO {@DTO} | User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    deactivateAccount,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong, try again later.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.Now;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[DeactivateAccount Method] =>
                         [IP] {IP}
                         User with [ID] {ID} could not deactivate his profile due to errors.{@UpdateErrors}
                         DTO {@DTO} | User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    updateResult,
                    deactivateAccount,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong while deactivating the account",
                    updateResult.Errors.Select(x => x.Description).ToList(),
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Information,
                """
                    [UserManagment Module]-[AccountManagment Class]-[DeactivateAccount Method],
                    [IP] {IP}
                    User with [ID] : {ID} deactivated his account at {DeletedAt}
                    DTO {@DTO} | User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                id,
                user.DeletedAt,
                deactivateAccount,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Your account was deactivated successfully",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                     Something went wrong in [UserManagment Module]-[AccountManagment Class]-[DeactivateAccount Method],
                     user with [ID] {id}".
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Reactivate the account request
    /// </summary>
    /// <param name="email"> The email of the user </param>
    /// <returns> A generic response indicating when the user activation request was successfully or not </returns>
    public async Task<Util_GenericResponse<bool>>
    ActivateAccountRequest
    (
        string email
    )
    {
        var user = await GetApplicationUserByEmail(email);

        if (user is null || user.Email is null)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [UserManagment Module]- [AccountManagment Class]-[ActivateAccountRequest Method] =>
                    User with [IP] {IP} and
                    [Email] {Email} tried to activate the account. User doesn't exists.
                    User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        if (!user.IsDeleted)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                     [UserManagment Module]- [AccountManagment Class]-[ActivateAccountRequest Method] =>
                     User with [IP] {IP} and
                     [Email] {Email} tried to activate the account. User account is already active.
                     User {@User}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "Your account is already active.",
                null,
                System.Net.HttpStatusCode.BadRequest
            );
        }

        try
        {
            var token = await userManager.GenerateUserTokenAsync(user, "Default", activateAccountSettings.Value.Reason);

            if (String.IsNullOrEmpty(token))
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]- [AccountManagment Class]-[ActivateAccountRequest Method] =>
                         User with [IP] {IP} and
                         [Email] {Email} tried to activate the account. Token was not generated.
                         User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    email,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "There was an error, try again.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            var route = activateAccountSettings.Value.Route.Replace("{token}", encodedToken).Replace("{email}", email);

            var sendEmailResult = await Util_Email.SendActivateAccountEmail(user.Email, user.FullName, route);

            if (!sendEmailResult.IsSuccessStatusCode)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]- [AccountManagment Class]-[ActivateAccountRequest Method] =>
                         User with [IP] {IP} and
                         [Email] {Email} tried to activate the account. Email was not sent.
                         EmailResult {@EmailResult} | User {User} ,
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    email,
                    await sendEmailResult.DeserializeResponseBodyAsync(sendEmailResult.Body),
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "There was an error in sending the email, try again.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Information,
                """
                     [UserManagment Module]- [AccountManagment Class]-[ActivateAccountRequest Method] =>
                     User with [IP] {IP} and
                     [Email] {Email} tried to activate the account. Email was successfully sent.
                     User {@User} 
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "An email was sent to you for account reactivation",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                     Something went wrong in [UserManagment Module] - [ActivateAccountRequest Method],   +
                     user with [Email] {email} tried to make a request to reactivate the account ,
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Confirms the account activation
    /// </summary>
    /// <param name="accountConfirmation"> The <see cref=DTO_ActivateAccountConfirmation""/> object </param>
    /// <returns> A generic response indicating if the users account was activated or not </returns>
    public async Task<Util_GenericResponse<bool>>
    ActivateAccountConfirmation
    (
        DTO_ActivateAccountConfirmation accountConfirmation
    )
    {
        var user = await GetApplicationUserByEmail(accountConfirmation.Email);

        if (user is null)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [UserManagment Module]- [AccountManagment Class]-[ActivateAccountConfirmation Method] =>
                    User with [IP] {IP} and
                    [Email] {Email} tried to confirm the activation of the account. User doesn't exists.
                    DTO {@DTO} | User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                accountConfirmation.Email,
                accountConfirmation,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        if (!user.IsDeleted)
        {
            _logger.Log
            (
               LogLevel.Error,
               """
                    [UserManagment Module]- [AccountManagment Class]-[ActivateAccountConfirmation Method] =>
                    User with [IP] {IP} and
                    [Email] {Email} tried to activate the account. User account is already active.
                    DTO {@DTO} | User {@User}.
                """,
               await Util_GetIpAddres.GetLocation(_httpContextAccessor),
               accountConfirmation.Email,
               accountConfirmation,
               user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "Your account is already active.",
                null,
                System.Net.HttpStatusCode.BadRequest
            );
        }

        try
        {
            var validToken = await userManager.VerifyUserTokenAsync
            (
                user,
                "Default",
                activateAccountSettings.Value.Reason,
                accountConfirmation.Token
            );

            if (!validToken)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]- [AccountManagment Class]-[ActivateAccountConfirmation Method] =>
                        User with [IP] {IP} and
                        [Email] {Email} tried to activate the account. Token is not valid.
                        DTO {@DTO} | User {@User} | Token {Token}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    accountConfirmation.Email,
                    accountConfirmation,
                    user,
                    accountConfirmation.Token
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong with the account confirmation, try again",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            user.IsDeleted = false;
            user.CreatedAt = DateTime.Now;
            user.ModifiedAt = DateTime.Now;
            user.DeletedAt = null;

            var updateUserResult = await userManager.UpdateAsync(user);

            if (!updateUserResult.Succeeded)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                        [UserManagment Module]- [AccountManagment Class]-[ActivateAccountConfirmation Method] =>
                        User with [IP] {IP} and
                        [Email] {Email} tried to activate the account. Update user failed {@updateUserResult}
                        DTO {@DTO} | User {@User}.
                    """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   accountConfirmation.Email,
                   updateUserResult,
                   accountConfirmation,
                   user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong with the account confirmation, try again",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Error,
                """
                    [UserManagment Module]- [AccountManagment Class]-[ActivateAccountConfirmation Method] =>
                    User with [IP] {IP} and
                    [Email] {Email} tried to activate the account. Account updated successfully
                    DTO {@DTO} | User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                accountConfirmation.Email,
                accountConfirmation,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Your account was successfully activated.",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
           (
               ex,
               _logger,
               $"""
                    Something went wrong in [UserManagment Module] - [ActivateAccountConfirmation Method],
                    user with [Email] {accountConfirmation.Email} tried to confirm his reactivate the account request,
                """,
               false,
               _httpContextAccessor
           );
        }
    }
    /// <summary>
    /// Changes the password of a user based on the provided user ID and password details.
    /// </summary>
    /// <param name="id">The ID of the user whose password needs to be changed.</param>
    /// <param name="updatePassword">The details containing the old and new password information.</param>
    /// <returns>A generic response indicating whether the password was successfully changed or not.</returns>
    public async Task<Util_GenericResponse<bool>>
    ChangePassword
    (
        Guid id,
        DTO_UserChangePassword updatePassword
    )
    {
        try
        {
            var user = await GetApplicationUser(id);

            if (user is null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[ChangePassword Method] =>
                         [IP] {IP}
                         User with [ID] {ID} doesn't exists User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            var updatePasswordResult = await userManager.ChangePasswordAsync(user, updatePassword.OldPassword, updatePassword.ConfirmNewPassword);

            if (!updatePasswordResult.Succeeded)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[ChangePassword Method] =>
                         [IP] {IP}
                         User with [ID] {ID} could not update his password due to errors.
                         {@updatePasswordResult.Errors} User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id,
                    updatePasswordResult,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong while trying to update the password.",
                    updatePasswordResult.Errors.Select(x => x.Description).ToList(),
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            user.ModifiedAt = DateTime.Now;
            await userManager.UpdateAsync(user);

            _logger.Log
            (
                LogLevel.Information,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ChangePassword Method] =>
                     [IP] {IP},
                     User with [ID] {ID} just changed his password. User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                id,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Password updated successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                """
                     Something went wrong in [UserManagment Module]-[AccountManagment Class]-[ChangePassword Method],
                     user with [ID] {id} 
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Send an email to the user email with the link to reset his password.
    /// </summary>
    /// <param name="email"> The email of the user </param>
    /// <returns> A generic result indicating the result of the operation </returns>
    public async Task<Util_GenericResponse<bool>>
    ForgotPassword
    (
        string email
    )
    {
        var user = await GetApplicationUserByEmail(email);

        if (user is null || user.Email == null || user.IsDeleted)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ForgotPassword Method] =>
                     [IP] {IP} user with email {Email} doesn't exists. User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        if (user.IsDeleted)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ForgotPassword Method] =>
                     [IP] {IP} user with email {Email} doesn't exists. User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        try
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = System.Net.WebUtility.UrlEncode(token);

            var route = resetPasswordSettings.Value.Route.Replace("{token}", encodedToken).Replace("{email}", email);

            var sendEmailResult = await Util_Email.SendForgotPassordTokenEmail(user.Email, user.FullName, route);

            if (!sendEmailResult.IsSuccessStatusCode)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[ForgotPassword Method] =>
                         [IP] {IP}
                         user with email {Email} doesn't exists. User {@User}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    email,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Something went wrong in email sending.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Information,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ForgotPassword Method] =>
                     [IP] {IP}
                     An email was sent to {Email} for password restore. User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "An email has been sent to you with the link to restore the password.",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [UserManagment Module]-[AccountManagment Class]-[ForgotPassword Method], 
                    user with [ID] {user.Id}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Reset the password of the user.
    /// </summary>
    /// <param name="resetPassword">The reset password object</param>
    /// <returns>A generic response indicating the result of the operation</returns>
    public async Task<Util_GenericResponse<bool>>
    ResetPassword
    (
        DTO_ResetPassword resetPassword
    )
    {
        var user = await GetApplicationUserByEmail(resetPassword.Email);

        if (user == null || user.IsDeleted)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ResetPassword Method] =>
                     [IP] {IP}
                     user with email {Email} doesn't exists. User {@User} .
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                resetPassword.Email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        if (user.IsDeleted)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [UserManagment Module]-[AccountManagment Class]-[ResetPassword Method] =>
                    [IP] {IP} user with email {Email} doesn't exists.
                    User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                resetPassword.Email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "User doesn't exists",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }

        try
        {
            var changePasswordResult = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                _logger.Log
               (
                    LogLevel.Error,
                    """
                         [UserManagment Module]-[AccountManagment Class]-[ResetPassword Method] =>
                         [IP] {IP} user with email {Email} tried to
                         reset the password but failed : {@Errors}. User {@User} .
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    resetPassword.Email,
                    changePasswordResult.Errors,
                    user
               );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Password was not restored",
                    changePasswordResult.Errors.Select(x => x.Description).ToList(),
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Information,
                """
                     [UserManagment Module]-[AccountManagment Class]-[ResetPassword Method] => 
                     [IP] {IP} user with email {Email} successfully
                     reset the password. User {@User}.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                resetPassword.Email,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Password was successfully restored",
                changePasswordResult.Errors.Select(x => x.Description).ToList(),
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Somewthing went wrong in [UserManagment Module]-[AccountManagment Class]-[ResetPassword Method], 
                    user with [ID] {user.Id}
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Request for changing the email
    /// </summary>
    /// <param name="newEmailAddressDto">The <see cref="DTO_ChangeEmailAddressRequest"/> object dto </param>
    /// <returns>A generic response indicating the result of the operation</returns>
    public async Task<Util_GenericResponse<bool>>
    RequestChangeEmailAddress
    (
        Guid userId,
        DTO_ChangeEmailAddressRequest newEmailAddressDto
    )
    {
        try
        {
            var user = await GetApplicationUser(userId);

            if (user is null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[RequestChangeEmailAddress Method] =>
                        [IP] {IP}
                        user with email {CurrentEmailAddress} doesn't exists. DTO {@DTO} | User {@User}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    newEmailAddressDto.CurrentEmailAddress,
                    newEmailAddressDto,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            if (user.Email != newEmailAddressDto.CurrentEmailAddress)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[ChangeEmailAddress Method] => 
                        [IP] {IP} 
                        user with email {CurrentEmailAddress} is not correct. DTO {@DTO} | User {@User}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    newEmailAddressDto.CurrentEmailAddress,
                    newEmailAddressDto,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Your current email address is incorrect",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            if (userManager.Users.Any(x => x.Email == newEmailAddressDto.ConfirmNewEmailAddress))
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[RequestChangeEmailAddress Method] => 
                        [IP] {IP} user with email {CurrentEmailAddress}
                        tried to change email to an email that exists in the database.
                        DTO {@DTO} | User {@User}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    newEmailAddressDto.CurrentEmailAddress,
                    newEmailAddressDto,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Email is taken",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            var tokenForEmailConfirmation = await userManager.GenerateChangeEmailTokenAsync(user, newEmailAddressDto.ConfirmNewEmailAddress);

            var encodedToken = System.Net.WebUtility.UrlEncode(tokenForEmailConfirmation);

            var route = changeEmailAddressSettings.Value.Route.Replace("{token}", encodedToken).Replace("{email}", newEmailAddressDto.ConfirmNewEmailAddress);

            var sendEmailResult = await Util_Email.SendEmailForEmailConfirmation(newEmailAddressDto.ConfirmNewEmailAddress, route, user.FullName);

            if (!sendEmailResult.IsSuccessStatusCode)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                        [UserManagment Module]-[AccountManagment Class]-[RequestChangeEmailAddress Method] => 
                        [IP] {IP} user with email {CurrentEmailAddress} 
                        tried to change his email address and failed {@sendEmailResult} | {@sendEmailResultBody}.
                        DTO {@DTO} | User {@User}
                    """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    newEmailAddressDto.CurrentEmailAddress,
                    await sendEmailResult.DeserializeResponseBodyAsync(sendEmailResult.Body),
                    sendEmailResult.Body.ReadAsStringAsync().Result,
                    newEmailAddressDto,
                    user
                );

                return Util_GenericResponse<bool>.Response
                (
                    false,
                    false,
                    "Your old email address is incorrect",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            _logger.Log
            (
                LogLevel.Information,
                """
                    [UserManagment Module]-[AccountManagment Class]-[RequestChangeEmailAddress Method] => 
                    [IP] {IP} user with email {CurrentEmailAddress} 
                    requested to change the email address.
                    DTO {@DTO} | User {@User}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                newEmailAddressDto.CurrentEmailAddress,
                newEmailAddressDto,
                user
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "An email has been sent to your new email.",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Somewthing went wrong in [UserManagment Module]-[ChangeEmailAddress Class]-[RequestChangeEmailAddress Method],
                    user with [ID] {userId} and [Email] {newEmailAddressDto.CurrentEmailAddress}
                 """,
                false,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Confirms the request of changing the email.
    /// </summary>
    /// <param name="changeEmailAddressConfirmDto"> The <see cref="DTO_ChangeEmailAddressRequestConfirm"/> object dto </param>
    /// <returns>A generic response indicating the result of the operation</returns>
    public async Task<Util_GenericResponse<DTO_Token>>
    ConfirmChangeEmailAddressRequest
    (
        Guid userId,
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    )
    {
        try
        {
            var user = await GetApplicationUser(userId);

            if (user == null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[ConfirmRequestChangeEmailAddress Method] => 
                        [IP] {IP} user with email {EmailAddress}
                        tried to confirm the request for changing the email but the id was not extracted from the token.
                        DTO {@DTO} | User {@User}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    changeEmailAddressConfirmDto.EmailAddress,
                    changeEmailAddressConfirmDto,
                    user
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Something went wrong, please log in and try again.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            user.ModifiedAt = DateTime.Now;

            var confirmTokenResult = await userManager.ChangeEmailAsync(user, changeEmailAddressConfirmDto.EmailAddress, changeEmailAddressConfirmDto.Token);

            if (!confirmTokenResult.Succeeded)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                        [UserManagment Module]-[AccountManagment Class]-[ConfirmRequestChangeEmailAddress Method] => 
                        [IP] {IP} user with email {EmailAddress}
                        tried to confirm the request for changing the email but confirmTokenResult failed with {@ErrorsDescription}.
                        DTO {@DTO} | User {@User}
                    """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   changeEmailAddressConfirmDto.EmailAddress,
                   confirmTokenResult.Errors.Select(x => x.Description),
                   changeEmailAddressConfirmDto,
                   user
                );

                return Util_GenericResponse<DTO_Token>.Response
                (
                    null,
                    false,
                    "Something went wrong, your email was not verified succsessfully",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            await DeleteUserRefreshTokens(user.Id);

            var token = await GetToken(user);

            _logger.Log
            (
                LogLevel.Information,
                """
                    [UserManagment Module]-[AccountManagment Class]-[ConfirmRequestChangeEmailAddress Method] => 
                    [IP] {IP} user with email {EmailAddress} successfully changed his email.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                changeEmailAddressConfirmDto.EmailAddress
            );

            return Util_GenericResponse<DTO_Token>.Response
            (
                token,
                true,
                "Your email was successfully changed.",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_Token, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [UserManagment Module]-[ChangeEmailAddress Class]-[ConfirmRequestChangeEmailAddress Method],
                    user with [ID] {userId} and [Email] {changeEmailAddressConfirmDto.EmailAddress} tried to confirm the request to confirm the email.
                 """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Search usesr by their username
    /// </summary>
    /// <param name="userName">The username of the user</param>
    /// <param name="userId">The id of the user</param>
    /// <returns>A generic response indicating the result of the operation</returns>
    public async Task<Util_GenericResponse<List<DTO_UserSearched>>>
    SearchUserByUserName
    (
        string userName,
        string userId
    )
    {
        try
        {
            List<ApplicationUser>? users = await _db.Users.Include(x => x.Shops)
                                       .ThenInclude(x => x.ShopShares)
                                       .Where(x => x.UserName!.Contains(userName) && x.Id != userId && !x.IsDeleted && x.EmailConfirmed && x.Id != userId && !String.IsNullOrEmpty(x.PublicKey) && !String.IsNullOrEmpty(x.Signature) && !String.IsNullOrEmpty(x.SigningPublicKey))
                                       .ToListAsync();

            List<ApplicationUser> usersInRoleUser = [];

            foreach (ApplicationUser user in users)
            {
                if (await userManager.IsInRoleAsync(user, Role.User.ToString()))
                    usersInRoleUser.Add(user);
            }

            return Util_GenericResponse<List<DTO_UserSearched>>.Response
            (
                usersInRoleUser.Select(_mapper.Map<DTO_UserSearched>).ToList(),
                true,
                "User retrieved successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<List<DTO_UserSearched>, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [UserManagment Module]-[ChangeEmailAddress Class]-[ConfirmChangeEmailAddressRequest Method],
                    user with [ID] {userId} tried to search user by username.
                 """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Uploads an image for a user
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="image"> The image of the user</param>
    /// <returns>A generic response indicating the result of the operation</returns>
    public async Task<Util_GenericResponse<byte[]>>
    UploadProfilePicture
    (
        Guid userId,
        IFormFile? image
    )
    {
        try
        {
            if (image is null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[UploadProfilePicture Method] => 
                        [IP] {IP} user with email {Id} tried to upload a profile picture. Null img obj => {imgobj}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    userId,
                    image
                );

                return Util_GenericResponse<byte[]>.Response
                (
                    null,
                    false,
                    "Something went wrong, please upload again.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            var user = await GetApplicationUser(userId);

            if (user == null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [UserManagment Module]-[AccountManagment Class]-[UploadProfilePicture Method] => 
                        [IP] {IP} user with email {Id}  tried to upload a profile picture.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    userId
                );

                return Util_GenericResponse<byte[]>.Response
                (
                    null,
                    false,
                    "Something went wrong, please log in and try again.",
                    null,
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            if (!IsImageFile(image.ContentType))
            {
                _logger.Log
               (
                   LogLevel.Error,
                   """
                        [UserManagment Module]-[AccountManagment Class]-[UploadProfilePicture Method] => 
                        [IP] {IP} user with email {Id} tried to upload a wrong format profile picture.
                        Image obj {imgObj}
                    """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   userId,
                   image
               );

                return Util_GenericResponse<byte[]>.Response
                (
                    null,
                    false,
                    "Unsupported file format!",
                    [
                        "Unsupported file format!"
                    ],
                    System.Net.HttpStatusCode.BadRequest
                );
            }

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var hexImageData = BitConverter.ToString(imageData).Replace("-", "");

                var convertedImageData = StringToByteArray(hexImageData);

                user.ImageData = convertedImageData;

                await _db.SaveChangesAsync();
            }

            return Util_GenericResponse<byte[]>.Response
            (
                user.ImageData,
                true,
                "User profile picture uploaded successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<byte[], AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                Somewthing went wrong in [UserManagment Module]-[ChangeEmailAddress Class]-[UploadProfilePicture Method],
                user with [ID] {userId} tried to upload a profile picture.
                """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Checks if the image is of allowed formats
    /// </summary>
    /// <param name="contentType"> The file content of the image</param>
    /// <returns> True or false </returns>
    private static bool IsImageFile
    (
        string contentType
    )
    {
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
        return allowedTypes.Contains(contentType);
    }
    /// <summary>
    /// convert hexadecimal string to byte array
    /// </summary>
    /// <param name="hex">The hex value</param>
    /// <returns>String representation of the hex</returns>
    private static byte[]
    StringToByteArray
    (
        string hex
    )
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }
    /// <summary>
    /// Retrieves the user details mapped to DTO based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to be retrieved.</param>
    /// <returns>A response containing user details in DTO format.</returns>
    private async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    GetUserInfoMapped
    (
        string purpose,
        Guid id,
        DTO_Token? userToken
    )
    {
        try
        {
            var user = await GetApplicationUser(id);

            if (user is null || user.IsDeleted)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                          [UserManagment Module]-[AccountManagment Class]-[GetUserInfoMapped Method] =>
                          [IP] {IP}
                          User with [ID] {ID} doesn't exists 
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    id
                );

                return Util_GenericResponse<DTO_UserUpdatedInfo>.Response
                (
                    null,
                    false,
                    "User doesn't exists",
                    null,
                    System.Net.HttpStatusCode.NotFound
                );
            }

            var userInfoMapped = _mapper.Map<DTO_UserUpdatedInfo>(user);
            userInfoMapped.UserToken = userToken;

            userInfoMapped.Role = await GetRole(user);

            return Util_GenericResponse<DTO_UserUpdatedInfo>.Response
            (
                userInfoMapped,
                true,
                $"User {purpose} successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_UserUpdatedInfo, AccountManagment>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [UserManagment Module]-[AccountManagment Class]-[GetUserInfoMapped Method], 
                    user with [ID] {id}
                 """,
                null,
                _httpContextAccessor
            );
        }
    }
    /// <summary>
    /// Get user role
    /// </summary>
    /// <param name="user">Logged in user obj</param>
    /// <returns>User Role</returns>
    private async Task<Role>
    GetRole
    (
        ApplicationUser user
    )
    {
        var roles = await userManager.GetRolesAsync(user);

        if (roles.FirstOrDefault() == Role.Moderator.ToString())
            return Role.Moderator;

        return Role.User;
    }
    /// <summary>
    /// Fetches the ApplicationUser from the persistence store based on the provided user ID.
    /// </summary>
    /// <param name="id">The ID of the user to be fetched.</param>
    /// <returns>The ApplicationUser if found, null otherwise.</returns>
    private async Task<ApplicationUser?>
    GetApplicationUser
    (
        Guid id
    )
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        return user;
    }
    /// <summary>
    /// Fetches the ApplicationUser from the persistence store based on the provided user email.
    /// </summary>
    /// <param name="email">The email of the user to be fetched.</param>
    /// <returns>The ApplicationUser if found, null otherwise.</returns>
    private async Task<ApplicationUser?>
    GetApplicationUserByEmail
    (
        string email
    )
    {
        var user = await userManager.FindByEmailAsync(email);

        return user;
    }
    /// <summary>
    /// Get the jwt token.
    /// </summary>
    /// <param name="user">The <see cref="ApplicationUser"/> object </param>
    /// <returns> The Jwt token </returns>
    private async Task<DTO_Token>
    GetToken
    (
        ApplicationUser user
    )
    {
        var roles = await userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<DTO_AuthUser>(user);
        userDto.Roles = roles.ToList();
        var token = await jwtTokenService.CreateToken(userDto);

        return token;
    }
    /// <summary>
    /// Deletes users refresh tokens 
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>A async task</returns>
    private async Task
    DeleteUserRefreshTokens
    (
        string userId
    )
    {
        var userRefreshTokens = await _db.RefreshTokens.Include(x => x.User)
                                                         .Where(x => x.UserId == userId)
                                                         .ToArrayAsync();

        _db.RefreshTokens.RemoveRange(userRefreshTokens);

        await _db.SaveChangesAsync();
    }
}