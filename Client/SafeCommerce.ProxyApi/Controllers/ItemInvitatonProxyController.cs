using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default", Roles = "User")]
public class ItemInvitatonProxyController
(
    IItemInvitationProxyService _itemInvitationProxyService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    [HttpGet(Route_InvitationRoute.ProxyGetItemsInvitations)]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_RecivedItemInvitation>>>> 
    GetShopsInvitations()
    {
        var result = await _itemInvitationProxyService.GetItemsInvitations
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<List<DTO_RecivedItemInvitation>>.ControllerResponse(result);
    }

    [HttpGet(Route_InvitationRoute.ProxyGetSentItemsInvitations)]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_SentItemInvitation>>>> 
    GetSentItemInvitations()
    {
        var result = await _itemInvitationProxyService.GetSentItemsInvitations
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<List<DTO_SentItemInvitation>>.ControllerResponse(result);
    }

    [HttpPost(Route_InvitationRoute.ProxySendItemInvitation)]
    public async Task<ActionResult<Util_GenericResponse<bool>>> 
    SendItemInvitation
    (
        [FromBody] DTO_SendItemInvitationRequest dTO_SendInvitation
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemInvitationProxyService.SendItemInvitation
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            dTO_SendInvitation
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_InvitationRoute.ProxyAcceptItemInvitation)]
    public async Task<ActionResult<Util_GenericResponse<bool>>> 
    AcceptItemInvitation
    (
        [FromBody] DTO_InvitationItemRequestActions acceptInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemInvitationProxyService.AcceptItemInvitation
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            acceptInvitationRequest
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_InvitationRoute.ProxyRejectItemInvitation)]
    public async Task<ActionResult<Util_GenericResponse<bool>>> 
    RejectInvitation
    (
        [FromBody] DTO_InvitationItemRequestActions rejectInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemInvitationProxyService.RejectItemInvitation
        (

            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            rejectInvitationRequest
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpDelete(Route_InvitationRoute.ProxyDeleteItemInvitation)]
    public async Task<ActionResult<Util_GenericResponse<bool>>> 
    DeleteInvitation
    (
        [FromBody] DTO_InvitationItemRequestActions deleteInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemInvitationProxyService.DeleteItemInvitation
        (
            API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            deleteInvitationRequest
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}