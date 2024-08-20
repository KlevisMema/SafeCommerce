using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.API.ActionFilters;
using SafeCommerce.API.Helpers.AttributeFilters;
using SafeCommerce.MediatR.Actions.Queries.Shop;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.API.Controllers;

/// <summary>
/// Controller for managing shops in the SafeShare application.
/// This controller includes functionalities for shop creation, modification, deletion, 
/// and retrieval of shop details.
/// It communicates with the business logic layer using MediatR for a clean, decoupled architecture.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="ShopController"/>
/// </remarks>
/// <param name="mediator">The instance of mediator used to send commands and queries</param>
[ServiceFilter(typeof(VerifyUser))]
public class ShopController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>
    /// Retrieves details of a specific shop.
    /// </summary>
    /// <param name="shopId">The identifier of the shop whose details are being requested.</param>
    /// <param name="userId">The identifier of the user requesting shop details.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Detailed information about the specified shop.</returns>
    [Authorize(Roles = "User")]
    [HttpGet(Route_ShopRoutes.GetShop)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
    GetShopById
    (
        [FromRoute] Guid shopId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetShopByIdQuery(shopId, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Retrieves all public shared shops.
    /// </summary>
    /// <param name="userId">The identifier of the user requesting shop details.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Detailed information about the specified shop.</returns>
    [Authorize(Roles = "User")]
    [HttpGet(Route_ShopRoutes.GetPublicShops)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_Shop>))]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Shop>>>>
    GetPublicShops
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetShopsQuery(userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Creates a new shop.
    /// </summary>
    /// <param name="userId">The identifier of the user creating the shop.</param>
    /// <param name="createShopDto">The details of the shop to be created.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpPost(Route_ShopRoutes.CreateShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
    CreateShop
    (
        [FromRoute] Guid userId,
        [FromForm] DTO_CreateShop createShopDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_CreateShopCommand(createShopDto, userId.ToString()));
    }

    /// <summary>
    /// Edits the details of an existing shop.
    /// </summary>
    /// <param name="shopId">The identifier of the shop to be edited.</param>
    /// <param name="userId">The identifier of the user editing the shop.</param>
    /// <param name="editShopDto">The new details to update the shop with.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpPut(Route_ShopRoutes.EditShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Shop>>>
    EditShop
    (
        [FromRoute] Guid shopId,
        [FromRoute] Guid userId,
        [FromForm] DTO_UpdateShop editShopDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_UpdateShopCommand(shopId, editShopDto, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Deletes an existing shop.
    /// </summary>
    /// <param name="shopId">The identifier of the shop to be deleted.</param>
    /// <param name="userId">The identifier of the user deleting the shop.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpDelete(Route_ShopRoutes.DeleteShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeleteShop
    (
        [FromRoute] Guid shopId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_DeleteShopCommand(shopId, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Retrieves all shops for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose shops are to be retrieved.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpGet(Route_ShopRoutes.GetShops)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_Shop>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_Shop>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_Shop>>>>
    GetUserShops
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetUserShopsQuery(userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Invites a user to a shop.
    /// </summary>
    /// <param name="inviteUserToShopDto">The details of the user invitation.</param>
    /// <param name="userId">The identifier of the shop owner.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpPost(Route_ShopRoutes.InviteUserToShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    InviteUserToShop
    (
        [FromBody] DTO_InviteUserToShop inviteUserToShopDto,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_InviteUserToShopCommand(inviteUserToShopDto, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Gets all shops for moderation that are set to be set public or not.
    /// </summary>
    /// <param name="userId">The identifier of the moderator.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "Moderator")]
    [HttpGet(Route_ShopRoutes.GetShopsForModeration)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>))]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>>
    GetShopsSubjectForModeration
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_GetShopsSubjectForModerationQuery(userId), cancellationToken);
    }

    /// <summary>
    /// Moderates a shop by setting it as aproved to be public or not.
    /// </summary>
    /// <param name="userId">The identifier of the moderator.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="dTO_ModerateShop">Moderation object.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "Moderator")]
    [HttpPost(Route_ShopRoutes.ModerateShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ModerateShop
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_ModerateShop dTO_ModerateShop,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ModerateShopCommand(dTO_ModerateShop, userId), cancellationToken);
    }

    /// <summary>
    /// Removes a user form the shop
    /// </summary>
    /// <param name="userId">The identifier of the owner of the shop.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="dTO_RemoveUserFromShop">Data object.</param>
    /// <returns>A object value indicating whether the result was succsessful.</returns>
    [Authorize(Roles = "User")]
    [HttpDelete(Route_ShopRoutes.RemoveUserFromShop)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RemoveUserFromShop
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_RemoveUserFromShop dTO_RemoveUserFromShop,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_RemoveUserFromShopCommand(userId, dTO_RemoveUserFromShop), cancellationToken);
    }
}