#region Usings
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.JwtSecurity.Helpers;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.Moderation;
using Microsoft.AspNetCore.Authorization;
#endregion

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default", Roles = "User")]
public class ItemProxyController(
    IItemProxyService _itemProxyService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    #region Get
    [HttpGet(Route_ItemRoutes.ProxyGetItemDetails)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    GetItemDetails
    (
       [FromRoute] Guid itemId
    )
    {
        var result = await _itemProxyService.GetItemDetails
        (
            itemId,
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<DTO_Item>.ControllerResponse(result);
    }

    [HttpGet(Route_ItemRoutes.PeoxyGetItemsForModeration)]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Item>>>>
    GetItemsSubjectForModeration()
    {
        var result = await _itemProxyService.GetItemsSubjectForModeration
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<IEnumerable<DTO_Item>>.ControllerResponse(result);
    }

    [HttpGet(Route_ItemRoutes.ProxyGetItemsByShopId)]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_Item>>>>
    GetItemsByShopId
    (
        [FromRoute] Guid shopId
    )
    {
        var result = await _itemProxyService.GetItemsByShopId
        (
            shopId,
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<List<DTO_Item>>.ControllerResponse(result);
    }

    [HttpGet(Route_ItemRoutes.ProxyGetUserItems)]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Item>>>>
    GetUserItems()
    {
        var result = await _itemProxyService.GetUserItems
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<IEnumerable<DTO_Item>>.ControllerResponse(result);
    }
    #endregion

    #region Post
    [HttpPost(Route_ItemRoutes.ProxyCreateItem)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    CreateItem
    (
        [FromForm] DTO_CreateItem createItemDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.CreateItem
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            createItemDto
        );

        return Util_GenericControllerResponse<DTO_Item>.ControllerResponse(result);
    }

    [HttpPost(Route_ItemRoutes.ProxyShareItem)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ShareItem
    (
        [FromBody] DTO_ShareItem shareItemDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.ShareItem
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            shareItemDto
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }

    [HttpPost(Route_ItemRoutes.ProxyModerateItem)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ModerateItem
    (
        [FromBody] DTO_ModerateItem moderateItemDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.ModerateItem
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            moderateItemDto
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
    #endregion

    #region Put
    [HttpPut(Route_ItemRoutes.ProxyEditItem)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    EditItem
    (
       [FromRoute] Guid itemId,
       [FromForm] DTO_UpdateItem editItemDto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.EditItem
        (
            itemId,
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            editItemDto
        );

        return Util_GenericControllerResponse<DTO_Item>.ControllerResponse(result);
    }
    #endregion

    #region Delete
    [HttpDelete(Route_ItemRoutes.ProxyDeleteItem)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeleteItem
    (
        [FromRoute] Guid itemId
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.DeleteItem
        (
            itemId,
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request)
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
    #endregion
}