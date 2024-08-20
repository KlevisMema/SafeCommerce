/*
 * User Management Controller
 * 
 * This controller is responsible for all operations related to user management within the SafeCommerce application.
 * It provides endpoints to fetch user details, update user information, delete users, and change user passwords.
 * 
 * The controller leverages the MediatR library to dispatch requests to the appropriate handlers, 
 * ensuring a clean separation of concerns and promoting a CQRS pattern.
*/

using MediatR;
using SafeCommerce.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.API.ActionFilters;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.Security.User.Implementation;
using SafeCommerce.API.Helpers.AttributeFilters;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.MediatR.Actions.Queries.UserManagment;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.API.Controllers;

/// <summary>
/// Controller responsible for managing users, including fetching, updating, deleting, and changing passwords.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AccountManagmentController"/> class.
/// </remarks>
/// <param name="mediator">Mediator pattern handler.</param>
/// <param name="cookieOpt">Cookie Options</param>
/// <param name="_userDataProtection">Data protection service</param>
[Authorize(Roles = "User, Moderator")]
public class AccountManagmentController
(
    IMediator mediator,
    IOptions<API_Helper_CookieSettings> cookieOpt,
    ISecurity_UserDataProtectionService _userDataProtection
) : BaseController(mediator)
{
    /// <summary>
    /// Fetch a user's updated information by their ID.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <returns>Returns user's updated information.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpGet(Route_AccountManagmentRoute.GetUser)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_UserUpdatedInfo>>>
    GetUser
    (
        Guid userId
    )
    {
        return await _mediator.Send(new MediatR_GetUserQuery(userId));
    }
    /// <summary>
    /// Update a user's information.
    /// </summary>
    /// <param name="userId">Unique identifier of the user to be updated.</param>
    /// <param name="userInfo">User's new information.</param>
    /// <returns>Returns user's updated information.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPut(Route_AccountManagmentRoute.UpdateUser)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_UserUpdatedInfo>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_UserUpdatedInfo>>>
    UpdateUser
    (
        Guid userId,
        [FromForm] DTO_UserInfo userInfo
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _mediator.Send(new MediatR_UpdateUserCommand(userId, userInfo));

        if (result.Succsess && result.Value is not null && result.Value.UserToken is not null)
        {
            SetCookiesResposne(result.Value.UserToken, result.Value.UserID);
            result.Value.UserToken = null;
        }

        return Util_GenericControllerResponse<DTO_UserUpdatedInfo>.ControllerResponse(result);
    }
    /// <summary>
    /// Change a user's password by their ID.
    /// </summary>
    /// <param name="userId">Unique identifier of the user.</param>
    /// <param name="changePassword">Details for changing the user's password.</param>
    /// <returns>Returns a boolean indicating the success of the password change operation.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPut(Route_AccountManagmentRoute.ChangePassword)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ChangePassword
    (
        Guid userId,
        [FromForm] DTO_UserChangePassword changePassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ChangeUserPasswordCommand(userId, changePassword));
    }
    /// <summary>
    /// Deactivate a user by their ID.
    /// </summary>
    /// <param name="userId">Unique identifier of the user to be deactivated</param>
    /// <param name="deactivateAccount"> A dto containing user's information for deactivation process </param>
    /// <returns>Returns a boolean indicating the success of the deactivation operation.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AccountManagmentRoute.DeactivateAccount)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeactivateAccount
    (
        Guid userId,
        [FromForm] DTO_DeactivateAccount deactivateAccount
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _mediator.Send(new MediatR_DeactivateAccountCommand(userId, deactivateAccount));

        ClearCookies();

        return result;
    }
    /// <summary>
    /// Sends a request to activate an account based on the provided email address.
    /// </summary>
    /// <param name="email">The email address associated with the account to be activated.</param>
    /// <returns>A response indicating the success or failure of the account activation request.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ActivateAccountRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ActivateAccountRequest
    (
        string email
    )
    {
        return await _mediator.Send(new MediatR_ActivateAccountRequestCommand(email));
    }
    /// <summary>
    /// Confirms the account activation request using a specific confirmation token or code.
    /// </summary>
    /// <param name="activateAccountConfirmationDto">The DTO containing the confirmation details for account activation.</param>
    /// <returns>A response indicating the success or failure of the account activation confirmation.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ActivateAccountRequestConfirmation)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ActivateAccountRequestConfirmation
    (
        DTO_ActivateAccountConfirmation activateAccountConfirmationDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ActivateAccountConfirmationRequestCommand(activateAccountConfirmationDto));
    }
    /// <summary>
    /// Initiates the password reset process for a user based on their email address.
    /// </summary>
    /// <param name="forgotPassword">The object that contains the email address of a user</param>
    /// <returns>A response indicating the success or failure of the forgot password request.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ForgotPassword)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ForgotPassword
    (
       [FromForm] DTO_ForgotPassword forgotPassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ForgotPasswordCommand(forgotPassword.Email));
    }
    /// <summary>
    /// Allows a user to reset their password using a password reset token.
    /// </summary>
    /// <param name="resetPassword">The DTO containing the new password and reset token information.</param>
    /// <returns>A response indicating the success or failure of the password reset operation.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ResetPassword)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ResetPassword
    (
        [FromForm] DTO_ResetPassword resetPassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ResetUserPasswordCommand(resetPassword));
    }
    /// <summary>
    /// Sends a request to change the email address associated with a user's account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user requesting the email change.</param>
    /// <param name="emailAddress">The DTO containing the new email address information.</param>
    /// <returns>A response indicating the success or failure of the email change request.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AccountManagmentRoute.RequestChangeEmail)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RequestChangeEmail
    (
        Guid userId,
        [FromForm] DTO_ChangeEmailAddressRequest emailAddress
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ChangeEmailAddressRequestCommand(userId, emailAddress));
    }
    /// <summary>
    /// Confirms the email change request for a user's account.
    /// </summary>
    /// <param name="userId">The unique identifier of the user confirming the email change.</param>
    /// <param name="changeEmailAddressConfirmDto">The DTO containing confirmation details for the email change.</param>
    /// <returns>A response indicating the success or failure of the email change confirmation.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [HttpPost(Route_AccountManagmentRoute.ConfirmChangeEmailRequest)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ConfirmChangeEmailAddressRequest
    (
        Guid userId,
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _mediator.Send(new MediatR_ChangeEmailAddressRequestConfirmationCommand(userId, changeEmailAddressConfirmDto));

        if (result.Succsess && result.Value is not null)
        {
            SetCookiesResposne(result.Value, userId.ToString());
            result.Value.Token = null;
        }

        return Util_GenericControllerResponse<bool>.ControllerResponse(new Util_GenericResponse<bool>
        {
            Errors = result.Errors,
            Message = result.Message,
            StatusCode = result.StatusCode,
            Succsess = result.Succsess,
            Value = result.Succsess
        });
    }
    /// <summary>
    /// Search users by their usernames
    /// </summary>
    /// <param name="userName">The username</param>
    /// <param name="userId">The id of the user making the request</param>
    /// <returns>The response containing the list of the users</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpGet(Route_AccountManagmentRoute.SearchUserByUserName)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_UserSearched>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_UserSearched>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_UserSearched>>>>
    SearchUserByUserName
    (
        string userName,
        Guid userId
    )
    {
        return await _mediator.Send(new MediatR_SearchUsersQuery(userId, userName));
    }
    /// <summary>
    /// Uploads a profile picture  
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="image">The image content</param>
    /// <returns>A response indicating the success or failure the operation.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AccountManagmentRoute.UploadProfilePicture)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<byte[]>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<byte[]>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<byte[]>))]
    public async Task<ActionResult<Util_GenericResponse<byte[]>>>
    UploadProfilePicture
    (
        Guid userId,
        IFormFile image
    )
    {
        return await _mediator.Send(new MediatR_UploadProfilePictureCommand(userId, image));
    }
    /// <summary>
    /// Sets the cookies in the clients browser
    /// </summary>
    /// <param name="token">The generated values</param>
    /// <param name="userId">The id of the user</param>
    private void
    SetCookiesResposne
    (
        DTO_Token token,
        string userId
    )
    {

        HttpContext.Response.Cookies.Append
        (
            cookieOpt.Value.AuthTokenCookieName, token!.Token!,
            new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.ValididtyTime.AddHours(1),
            }
        );

        HttpContext.Response.Cookies.Append
        (
            cookieOpt.Value.RefreshAuthTokenCookieName, _userDataProtection.Protect(token.RefreshToken!, userId),
            new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.ValididtyTime.AddHours(1),
            }
        );

        HttpContext.Response.Cookies.Append
        (
            cookieOpt.Value.RefreshAuthTokenIdCookieName, _userDataProtection.Protect(token.RefreshTokenId!.ToString(), userId),
            new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = token.ValididtyTime.AddHours(1),
            }
        );

    }
    /// <summary>
    /// Clears all authentication and refresh tokens stored in cookies.
    /// </summary>
    private void
    ClearCookies()
    {
        ClearCookie("XSRF-TOKEN");
        ClearCookie(".AspNetCore.Identity.Application");
        ClearCookie(cookieOpt.Value.AuthTokenCookieName);
        ClearCookie(".AspNetCore.Antiforgery.NcD0snFZIjg");
        ClearCookie(cookieOpt.Value.RefreshAuthTokenCookieName);
        ClearCookie(cookieOpt.Value.RefreshAuthTokenIdCookieName);
    }
    /// <summary>
    /// Clears a specific cookie identified by its name.
    /// </summary>
    /// <param name="cookieName">The name of the cookie to be cleared.</param>
    private void
    ClearCookie
    (
        string cookieName
    )
    {
        HttpContext.Response.Cookies.Append(cookieName, "", new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        });
    }
}