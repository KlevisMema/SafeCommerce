#region Usings
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using Microsoft.AspNetCore.Authorization;
#endregion

namespace SafeCommerce.ProxyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Default")]
    public class ShopProxyController
    (
        IShopProxyService _shopProxyService,
        IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions
    ) : ControllerBase
    {
        #region Get
        /// <summary>
        /// Retrieves details of a specific shop.
        /// </summary>
        /// <param name="shopId">The identifier of the shop to be retrieved.</param>
        /// <returns>Detailed information about the specified shop.</returns>
        [Authorize(Roles = "User")]
        [HttpGet(Route_ShopRoutes.ProxyGetShop)]
        public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
        GetShopById
        (
            [FromRoute] Guid shopId
        )
        {
            var result = await _shopProxyService.GetShop
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                shopId,
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
            );

            return Util_GenericControllerResponse<DTO_Shop>.ControllerResponse(result);
        }

        /// <summary>
        /// Retrieves all shops for the current user.
        /// </summary>
        /// <returns>A list of shops associated with the current user.</returns>
        [Authorize(Roles = "User")]
        [HttpGet(Route_ShopRoutes.ProxyGetShops)]
        public async Task<ActionResult<Util_GenericResponse<List<DTO_Shop>>>>
        GetUserShops()
        {
            var result = await _shopProxyService.GetUserShops
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
            );

            return Util_GenericControllerResponse<List<DTO_Shop>>.ControllerResponse(result);
        }

        /// <summary>
        /// Retrieves all shops for the current user.
        /// </summary>
        /// <returns>A list of shops associated with the current user.</returns>
        [Authorize(Roles = "User")]
        [HttpGet(Route_ShopRoutes.ProxyGetPublicShops)]
        public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Shop>>>>
        GetPublicSharedShops()
        {
            var result = await _shopProxyService.GetPublicSharedShops
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
            );

            return Util_GenericControllerResponse<IEnumerable<DTO_Shop>>.ControllerResponse(result);
        }

        /// <summary>
        /// Retrieves all shops for moderation.
        /// </summary>
        /// <returns>A list of shops associated with the current moderator</returns>
        [Authorize(Roles = "Moderator")]
        [HttpGet(Route_ShopRoutes.ProxyGetShopsForModeration)]
        public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>>
        GetShopsSubjectForModeration()
        {
            var result = await _shopProxyService.GetShopsSubjectForModeration
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request)
            );

            return Util_GenericControllerResponse<IEnumerable<DTO_ShopForModeration>>.ControllerResponse(result);
        }
        #endregion

        #region Post
        /// <summary>
        /// Creates a new shop.
        /// </summary>
        /// <param name="createShopDto">The details of the shop to be created.</param>
        /// <returns>Details of the created shop.</returns>
        [Authorize(Roles = "User")]
        [HttpPost(Route_ShopRoutes.ProxyCreateShop)]
        public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
        CreateShop
        (
            [FromForm] DTO_CreateShop createShopDto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.CreateShop
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
                createShopDto
            );

            return Util_GenericControllerResponse<DTO_Shop>.ControllerResponse(result);
        }

        /// <summary>
        /// Invites a user to a shop.
        /// </summary>
        /// <param name="shopId">The identifier of the shop to which the user is being invited.</param>
        /// <param name="inviteUserToShopDto">The details of the user to be invited.</param>
        /// <returns>A boolean value indicating whether the user was successfully invited.</returns>
        [Authorize(Roles = "User")]
        [HttpPost(Route_ShopRoutes.ProxyInviteUserToShop)]
        public async Task<ActionResult<Util_GenericResponse<bool>>>
        InviteUserToShop
        (
            [FromRoute] Guid shopId,
            [FromBody] DTO_InviteUserToShop inviteUserToShopDto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.InviteUserToShop
            (
                shopId,
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
                inviteUserToShopDto
            );

            return Util_GenericControllerResponse<bool>.ControllerResponse(result);
        }

        /// <summary>
        /// Moderates a shop by setting it as approved or not.
        /// </summary>
        /// <param name="moderateShop">The details of the shop.</param>
        /// <returns>A boolean value indicating whether the shop was moderated or not.</returns>
        [Authorize(Roles = "Moderator")]
        [HttpPost(Route_ShopRoutes.ProxyModerateShop)]
        public async Task<ActionResult<Util_GenericResponse<bool>>>
        ModerateShop
        (
            [FromBody] DTO_ModerateShop moderateShop
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.MooderateShop
            (
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
                moderateShop
            );

            return Util_GenericControllerResponse<bool>.ControllerResponse(result);
        }
        #endregion

        #region Put
        /// <summary>
        /// Edits the details of an existing shop.
        /// </summary>
        /// <param name="shopId">The identifier of the shop to be edited.</param>
        /// <param name="editShopDto">The new details to update the shop with.</param>
        /// <returns>Details of the updated shop.</returns>
        [Authorize(Roles = "User")]
        [HttpPut(Route_ShopRoutes.ProxyEditShop)]
        public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
        EditShop
        (
            [FromRoute] Guid shopId,
            [FromForm] DTO_UpdateShop editShopDto
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.EditShop
            (
                shopId,
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
                editShopDto
            );

            return Util_GenericControllerResponse<DTO_Shop>.ControllerResponse(result);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Deletes an existing shop.
        /// </summary>
        /// <param name="shopId">The identifier of the shop to be deleted.</param>
        /// <returns>A boolean value indicating whether the shop was successfully deleted.</returns>
        [Authorize(Roles = "User")]
        [HttpDelete(Route_ShopRoutes.ProxyDeleteShop)]
        public async Task<ActionResult<Util_GenericResponse<bool>>>
        DeleteShop
        (
            [FromRoute] Guid shopId
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.DeleteShop
            (
                shopId,
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request)
            );

            return Util_GenericControllerResponse<bool>.ControllerResponse(result);
        }

        /// <summary>
        /// Removes a user from the shop
        /// </summary>
        /// <param name="dTO_RemoveUserFromShop">Object data</param>
        /// <returns>A boolean value indicating whether the user was successfully removed.</returns>
        [Authorize(Roles = "User")]
        [HttpDelete(Route_ShopRoutes.ProxyRemoveUserFromShop)]
        public async Task<ActionResult<Util_GenericResponse<bool>>>
        RemoveUserFromShop
        (
            [FromBody] DTO_RemoveUserFromShop dTO_RemoveUserFromShop
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _shopProxyService.RemoveUserFromShop
            (
                Guid.Parse(API_Helper_ExtractInfoFromRequestCookie.UserId(API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request))),
                API_Helper_ExtractInfoFromRequestCookie.GetUserIp(requestHeaderOptions.Value.ClientIP, Request),
                API_Helper_ExtractInfoFromRequestCookie.JwtToken(requestHeaderOptions.Value.AuthToken, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetForgeryToken(requestHeaderOptions.Value.Client_XSRF_TOKEN, Request),
                API_Helper_ExtractInfoFromRequestCookie.GetAspNetCoreForgeryToken(requestHeaderOptions.Value.AspNetCoreAntiforgery, Request),
                dTO_RemoveUserFromShop
            );

            return Util_GenericControllerResponse<bool>.ControllerResponse(result);
        }
        #endregion
    }
}