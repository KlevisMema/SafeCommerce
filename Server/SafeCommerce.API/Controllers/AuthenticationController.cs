/*
 * Defines the authentication controller for the SafeCommerce API.
 * This controller provides endpoints for user authentication processes including registration, login, and token management.
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
using SafeCommerce.Security.JwtSecurity.Helpers;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.MediatR.Actions.Queries.Authentication;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.API.Controllers;

/// <summary>
/// Controller for handling authentication processes in the SafeCommerce application.
/// Provides endpoints for user registration, login, confirmation, and token management functionalities.
/// Uses MediatR for command and query handling, facilitating a CQRS pattern.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthenticationController"/> class with the specified mediator.
/// </remarks>
/// <param name="mediator">The mediator used for command and query handling.</param>
/// <param name="cookieOpt">The options/settings from the config file</param>
/// <param name="_userDataProtection">The data protection provider used for encrypting and decrypting</param>
[ApiController]
[Route(BaseRoute.Route)]
public class AuthenticationController
(
    IMediator mediator,
    IOptions<API_Helper_CookieSettings> cookieOpt,
    ISecurity_UserDataProtectionService _userDataProtection
) : ControllerBase
{
    /// <summary>
    /// Endpoint to register a new user in the SafeCommerce system.
    /// Accepts user registration data and initiates the registration process.
    /// </summary>
    /// <param name="register">The registration data including user details.</param>
    /// <returns>A response indicating the success or failure of the registration process.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.Register)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    Register
    (
        [FromForm] DTO_Register register
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await mediator.Send(new MediatR_RegisterUserCommand(register));
    }
    /// <summary>
    /// Endpoint to confirm the registration of a new user.
    /// This typically involves verifying a token or a code sent to the user's email or phone.
    /// </summary>
    /// <param name="confirmRegistrationDto">The data required to confirm a user's registration.</param>
    /// <returns>A response indicating the success or failure of the registration confirmation process.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.ConfirmRegistration)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ConfirmRegistration
    (
        [FromBody] DTO_ConfirmRegistration confirmRegistrationDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await mediator.Send(new MediatR_ConfirmUserRegistrationCommand(confirmRegistrationDto));
    }
    /// <summary>
    /// Endpoint for user login.
    /// Validates user credentials and generates an authentication token upon successful login.
    /// </summary>
    /// <param name="loginDto">The user's login credentials.</param>
    /// <returns>A response containing the authentication token or an error message.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.Login)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_LoginResult>>>
    LoginUser
    (
        [FromForm] DTO_Login loginDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await mediator.Send(new MediatR_LoginUserCommand(loginDto));

        if (result.Succsess && result.Value is not null && result.Value.Token is not null)
        {
            if (result.Value.RequireOtpDuringLogin)
            {
                SetCookieResponse(result.Value.Token.Token!);
                result.Value.Token = null;
                return Ok(result);
            }

            SetCookiesResponse(result.Value.Token, result.Value.UserId);
            result.Value.Token = null;
        }

        return Util_GenericControllerResponse<DTO_LoginResult>.ControllerResponse(result);
    }
    /// <summary>
    /// Endpoint to confirm a user's login, typically used in two-factor authentication processes.
    /// Validates the OTP (One-Time Password) sent to the user after the initial login attempt.
    /// </summary>
    /// <param name="userId">The identifier of the user attempting to confirm login.</param>
    /// <param name="confirmLogin">The object fir user confirm login.</param>
    /// <returns>A response containing the authentication token or an error message.</returns>
    
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AuthenticationRoute.ConfirmLogin)]
    [Authorize(AuthenticationSchemes = "ConfirmLogin", Roles = "User, Moderator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_LoginResult>>>
    ConfirmLogin
    (
        Guid userId,
        DTO_ConfirmLogin confirmLogin
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await mediator.Send(new MediatR_ConfirmLoginUserCommand(confirmLogin.OTP, confirmLogin.UserId));

        if (result.Succsess && result.Value is not null && result.Value.Token is not null)
        {
            SetCookiesResponse(result.Value.Token, result.Value.UserId);
            result.Value.Token = null;
        }

        return Util_GenericControllerResponse<DTO_LoginResult>.ControllerResponse(result);
    }
    /// <summary>
    /// Saves users public key after successful log in.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userPublicKey">The public key of the user generated in the client</param>
    /// <returns>A response indicating the success or failure of the operation</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AuthenticationRoute.SaveUserPublicKey)]
    [Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_LoginResult>))]
    public async Task<ActionResult<Util_GenericResponse<string>>>
    SaveUsersPublicKey
    (
        Guid userId,
        [FromBody] DTO_SavePublicKey userPublicKey
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await mediator.Send(new Mediatr_SaveUsersPublicKeyCommand(userId, userPublicKey));

        return Util_GenericControllerResponse<string>.ControllerResponse(result);
    }
    /// <summary>
    /// Verify users public key after successful log in.
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <param name="userPublicKey">The public key of the user generated in the client</param>
    /// <returns>A response indicating the success or failure of the operation</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpGet(Route_AuthenticationRoute.VerifyPk)]
    [Authorize(AuthenticationSchemes = "Default")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    VerifyPK
    (
        Guid userId,
        [FromBody] string userPublicKey
    )
    {
        if (String.IsNullOrEmpty(userPublicKey))
            return BadRequest("Empty public key!");

        var result = await mediator.Send(new MediatR_VerifyPkQuery(userId, userPublicKey));

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
    /// <summary>
    /// Endpoint for requesting reconfirmation of the registration process.
    /// This can be used if the initial confirmation (like email verification) was not completed or needs to be resent.
    /// </summary>
    /// <param name="ReConfirmRegistration">The email address of the user requesting reconfirmation.</param>
    /// <returns>A response indicating the success or failure of the registration reconfirmation request.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.ReConfirmRegistrationRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ReConfirmRegistrationRequest
    (
        [FromBody] DTO_ReConfirmRegistration ReConfirmRegistration
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await mediator.Send(new MediatR_ReConfirmRegistrationRequestCommand(ReConfirmRegistration));
    }
    /// <summary>
    /// Endpoint for logging out a user.
    /// Performs actions like invalidating the user's token to ensure proper logout.
    /// </summary>
    /// <param name="userId">The identifier of the user who is logging out.</param>
    /// <returns>A response indicating the success or failure of the logout process.</returns>
    [ServiceFilter(typeof(VerifyUser))]
    [HttpPost(Route_AuthenticationRoute.LogOut)]
    [Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<ActionResult>
    LogOut
    (
        Guid userId
    )
    {
        await mediator.Send(new MediatR_LogOutCommand(userId.ToString()));

        ClearCookies();

        return Ok();
    }
    /// <summary>
    /// Endpoint for refreshing an expired authentication token.
    /// Validates the old token and issues a new one if the user is still authenticated.
    /// </summary>
    /// <returns>A response indicating the success or failure of the token refresh process and includes the new token if successful.</returns>
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.RefreshToken)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Token>>>
    RefreshToken()
    {
        try
        {
            string decryptedRefreshToken = "";
            Guid decryptedRefreshTokenId = Guid.Empty;

            var jwtToken = Request.Cookies.TryGetValue(cookieOpt.Value.AuthTokenCookieName, out string? authToken) ? authToken : string.Empty;

            string userId = Helper_JwtToken.GetUserIdDirectlyFromJwtToken(jwtToken);

            if (Request.Cookies.TryGetValue(cookieOpt.Value.RefreshAuthTokenCookieName, out string? refreshToken))
                decryptedRefreshToken = _userDataProtection.Unprotect(refreshToken, userId);

            if (Request.Cookies.TryGetValue(cookieOpt.Value.RefreshAuthTokenIdCookieName, out string? refreshTokenId))
            {
                var decryptedId = _userDataProtection.Unprotect(refreshTokenId, userId);
                decryptedRefreshTokenId = Guid.Parse(decryptedId);
            }

            var refreshTokenResult = await mediator.Send(new MediatR_RefreshTokenCommand(new DTO_ValidateToken
            {
                Token = jwtToken,
                RefreshToken = decryptedRefreshToken,
                RefreshTokenId = decryptedRefreshTokenId,
            }));

            if
            (
                refreshTokenResult.Succsess &&
                refreshTokenResult.Value is not null &&
                refreshTokenResult.Value.Token is not null
            )
            {
                SetCookiesResponse(refreshTokenResult.Value, userId);
            }

            refreshTokenResult.Value = null;

            return Util_GenericControllerResponse<DTO_Token>.ControllerResponse(refreshTokenResult);
        }
        catch (Exception)
        {
            ClearCookies();
            return Problem("Something went wrong in refresh token", null, 500, "Error", null);
        }
    }
    /// <summary>
    /// Sets the cookies in the clients browser
    /// </summary>
    /// <param name="token">The generated values</param>
    /// <param name="userId">The id of the user</param>
    private void
    SetCookiesResponse
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
    /// Sets a cookie in the HTTP response with a specific token.
    /// </summary>
    /// <param name="token">The token to be stored in the cookie.</param>
    private void
    SetCookieResponse
    (
       string token
    )
    {
        HttpContext.Response.Cookies.Append
        (
            cookieOpt.Value.AuthTokenCookieName, token,

            new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(5),
            }
        );
    }
    /// <summary>
    /// Clears all authentication and refresh tokens stored in cookies.
    /// </summary>
    private void
    ClearCookies()
    {
        string key = "XSRF-TOKEN-" + HttpContext.User.Identity.Name;
        string key2 = "Cookie-XSRF-TOKEN-" + HttpContext.User.Identity.Name;

        ClearCookie(key);
        ClearCookie(key2);
        ClearCookie(".AspNetCore.Identity.Application");
        ClearCookie(cookieOpt.Value.AuthTokenCookieName);
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