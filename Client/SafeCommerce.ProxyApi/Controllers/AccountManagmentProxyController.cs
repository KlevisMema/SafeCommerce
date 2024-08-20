#region Usings
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.UserManagment;
#endregion

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default", Roles = "User, Moderator")]
public class AccountManagmentProxyController
(
    IAccountManagmentProxyService accountManagmentProxyService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    #region Get
    [HttpGet(Route_AccountManagmentRoute.ProxyGetUser)]
    public async Task<ActionResult<Util_GenericResponse<DTO_UserUpdatedInfo>>>
    GetUser()
    {
        var result = await accountManagmentProxyService.GetUser
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );
        return Util_GenericControllerResponse<DTO_UserUpdatedInfo>.ControllerResponse(result);
    }

    [HttpGet(Route_AccountManagmentRoute.ProxySearchUserByUserName)]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_UserSearched>>>>
    SearchUserByUserName
    (
        string userName,
        CancellationToken cancellationToken
    )
    {
        var result = await accountManagmentProxyService.SearchUserByUserName
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            userName,
            cancellationToken
        );

        return Util_GenericControllerResponse<DTO_UserSearched>.ControllerResponseList(result);
    }
    #endregion

    #region Put
    [HttpPut(Route_AccountManagmentRoute.ProxyUpdateUser)]
    public async Task<ActionResult<Util_GenericResponse<DTO_UserUpdatedInfo>>>
    UpdateUser
    (
        [FromForm] DTO_UserInfo userInfo
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.UpdateUser
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            userInfo
        );

        return Util_GenericControllerResponse<DTO_UserUpdatedInfo>.ControllerResponse(result);
    }
    #endregion

    #region Post
    [HttpPut(Route_AccountManagmentRoute.ProxyChangePassword)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ChangePassword
    (
        [FromForm] DTO_UserChangePassword changePassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.ChangePassword
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            changePassword
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_AccountManagmentRoute.ProxyDeactivateAccount)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeactivateAccount
    (
        [FromForm] DTO_DeactivateAccount deactivateAccount
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.DeactivateAccount
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            deactivateAccount
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ActivateAccountRequest)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ActivateAccountRequest
    (
        string email
    )
    {
        var result = await accountManagmentProxyService.ActivateAccountRequest
        (
            email,
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request)
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ActivateAccountRequestConfirmation)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ActivateAccountRequestConfirmation
    (
        DTO_ActivateAccountConfirmation activateAccountConfirmationDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.ActivateAccountRequestConfirmation
        (
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            activateAccountConfirmationDto
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ForgotPassword)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ForgotPassword
    (
       [FromForm] DTO_ForgotPassword forgotPassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.ForgotPassword
        (
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            forgotPassword
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }


    [AllowAnonymous]
    [HttpPost(Route_AccountManagmentRoute.ResetPassword)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ResetPassword
    (
        [FromForm] DTO_ResetPassword resetPassword
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.ResetPassword
        (
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            resetPassword
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_AccountManagmentRoute.ProxyRequestChangeEmail)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RequestChangeEmail
    (
        [FromForm] DTO_ChangeEmailAddressRequest emailAddress
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.RequestChangeEmail
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            emailAddress
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_AccountManagmentRoute.ProxyConfirmChangeEmailRequest)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ConfirmChangeEmailAddressRequest
    (
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await accountManagmentProxyService.ConfirmChangeEmailAddressRequest
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            changeEmailAddressConfirmDto
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }



    [HttpPost(Route_AccountManagmentRoute.ProxyUploadProfilePicture)]
    public async Task<ActionResult<Util_GenericResponse<byte[]>>>
    UploadProfilePicture
    (
        IFormFile image
    )
    {
        var result = await accountManagmentProxyService.UploadProfilePicture
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            image
        );

        return Util_GenericControllerResponse<byte[]>.ControllerResponse(result);
    }
    #endregion
}