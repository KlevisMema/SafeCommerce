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
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.ProxyApi.Container.Services;
#endregion

namespace SafeCommerce.ProxyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Default")]
public class ItemProxyController(
    IItemProxyService _itemProxyService,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
) : ControllerBase
{
    #region Get
    [Authorize(Roles = "User")]
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

    [Authorize(Roles = "Moderator")]
    [HttpGet(Route_ItemRoutes.ProxyGetItemsForModeration)]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>>>
    GetItemsSubjectForModeration()
    {
        var result = await _itemProxyService.GetItemsSubjectForModeration
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<IEnumerable<DTO_ItemForModeration>>.ControllerResponse(result);
    }

    [Authorize(Roles = "User")]
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

    [Authorize(Roles = "User")]
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

    [Authorize(Roles = "User")]
    [HttpGet(Route_ItemRoutes.ProxyGetPublicSharedItems)]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_PublicItem>>>>
    GetPublicSharedItems()
    {
        var result = await _itemProxyService.GetPublicSharedItems
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<IEnumerable<DTO_PublicItem>>.ControllerResponse(result);
    }

    /// <summary>
    /// Retrieves all memebers of the a item.
    /// </summary>
    /// <param name="shopId">The id of the shop</param>
    /// <returns>A list of items associated with the current user.</returns>
    [Authorize(Roles = "User")]
    [HttpGet(Route_ShopRoutes.ProxyGetMembersOfTheItem)]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_ItemMembers>>>>
    GetMembersOfTheItem
    (
        [FromRoute] Guid itemId
    )
    {
        var result = await _itemProxyService.GetMembersOfTheItem
        (
            itemId,
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
        );

        return Util_GenericControllerResponse<IEnumerable<DTO_ItemMembers>>.ControllerResponse(result);
    }
    #endregion

    #region Post
    [Authorize(Roles = "User")]
    [HttpPost(Route_ItemRoutes.ProxyCreateItem)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    CreateItem
    (
        [FromBody] DTO_CreateItem createItemDto
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

    [Authorize(Roles = "User")]
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

    [Authorize(Roles = "Moderator")]
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
    [Authorize(Roles = "User")]
    [HttpPut(Route_ItemRoutes.ProxyEditItem)]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    EditItem
    (
       [FromRoute] Guid itemId,
       [FromBody] DTO_UpdateItem editItemDto
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
    [Authorize(Roles = "User")]
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

    /// <summary>
    /// Removes a user from the item
    /// </summary>
    /// <param name="dTO_RemoveUserFromShop">Object data</param>
    /// <returns>A boolean value indicating whether the user was successfully removed.</returns>
    [Authorize(Roles = "User")]
    [HttpDelete(Route_ShopRoutes.ProxyRemoveUserFromItem)]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RemoveUserFromShop
    (
        [FromBody] DTO_RemoveUserFromItem dTO_RemoveUserFromItem
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _itemProxyService.RemoveUserFromItem
        (
            Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
            API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
            API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
            API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
            dTO_RemoveUserFromItem
        );

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
    #endregion
}