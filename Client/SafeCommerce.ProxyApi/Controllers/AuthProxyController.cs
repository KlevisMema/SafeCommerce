#region Usings
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.DataTransormObject.UserManagment;

#endregion

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthProxyController
(
    IProxyAuthentication authenticationService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    #region Post
    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.Register)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    Register
    (
    [FromForm] DTO_Register register
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authenticationService.RegisterUser(register);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.ConfirmRegistration)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ConfirmRegistration
    (
        [FromBody] DTO_ConfirmRegistration confirmRegistrationDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authenticationService.ConfirmRegistration(confirmRegistrationDto);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.ReConfirmRegistrationRequest)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ReConfirmRegistrationRequest
    (
       [FromBody] DTO_ReConfirmRegistration reConfirmRegistration
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authenticationService.ReConfirmRegistrationRequest(reConfirmRegistration);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.Login)]
    public async Task<ActionResult<Util_GenericResponse<DTO_LoginResult>>>
    LoginUser
    (
        [FromForm] DTO_Login loginDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authenticationService.LogIn(loginDto);

        return Util_GenericControllerResponse<DTO_LoginResult>.ControllerResponse(result);
    }

    [HttpPost(Route_AuthenticationRoute.ConfirmLogin)]
    [Authorize(Roles = "User, Moderator")]
    public async Task<ActionResult<Util_GenericResponse<DTO_LoginResult>>>
    ConfirmLogin
    (
        Guid userId,
        DTO_ConfirmLogin confirmLogin
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var jwtToken = Request.Cookies["AuthToken"] ?? string.Empty;

        var result = await authenticationService.ConfirmLogin2FA(userId, jwtToken, confirmLogin);

        return Util_GenericControllerResponse<DTO_LoginResult>.ControllerResponse(result);
    }

    [Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
    [HttpPost(Route_AuthenticationRoute.SaveUserPublicKey)]
    public async Task<ActionResult<Util_GenericResponse<string>>>
    SaveUserPublicKey
    (
        [FromBody] DTO_SavePublicKey userPublicKey
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authenticationService.SaveUserPublicKey
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            userPublicKey
        );

        return Util_GenericControllerResponse<string>.ControllerResponse(result);
    }

    [HttpPost(Route_AuthenticationRoute.LogOut)]
    [Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
    public async Task<ActionResult>
    LogOut
    (

    )
    {
        var jwtToken = Request.Cookies["AuthToken"] ?? string.Empty;

        bool isUserId = Guid.TryParse(API_Helper_ExtractInfoFromRequestCookie.UserId
            (API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)), out var userId);

        if (!isUserId)
            return NotFound();

        await authenticationService.LogoutUser(userId, jwtToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost(Route_AuthenticationRoute.RefreshToken)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Token>>>
    RefreshToken()
    {
        var jwtToken = Request.Cookies["AuthToken"] ?? string.Empty;
        var refreshToken = Request.Cookies["RefreshAuthToken"] ?? string.Empty;
        var refreshTokenId = Request.Cookies["RefreshAuthTokenId"] ?? string.Empty;

        var result = await authenticationService.RefreshToken(jwtToken, refreshToken, refreshTokenId);

        if (!result.Succsess)
            ClearCookies();

        return Util_GenericControllerResponse<DTO_Token>.ControllerResponse(result);
    }
    #endregion

    #region Get
    [HttpGet(Route_AuthenticationRoute.JwtToken)]
    [Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
    public ActionResult<string>
    GetJwtToken()
    {
        return Ok(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request));
    }
    #endregion

    #region Private 
    private void
    ClearCookies()
    {
        ClearCookie(".AspNetCore.Identity.Application");
        ClearCookie("AuthToken");
        ClearCookie("RefreshAuthToken");
        ClearCookie("RefreshAuthTokenId");
        ClearCookie(".AspNetCore.Antiforgery.NcD0snFZIjg");
        ClearCookie("XSRF-TOKEN");
    }

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
    #endregion
}