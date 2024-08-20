/*
 * ItemController class for SafeShare application.
 * This controller handles item management functionalities including creating, editing, 
 * deleting items, and retrieving item details.
 * Utilizes the MediatR library for CQRS pattern implementation.
*/


using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.API.ActionFilters;
using SafeCommerce.API.Helpers.AttributeFilters;
using SafeCommerce.MediatR.Actions.Queries.Item;
using SafeCommerce.MediatR.Actions.Commands.Item;
using SafeCommerce.DataTransormObject.Moderation;
using Microsoft.AspNetCore.Authorization;

namespace SafeCommerce.API.Controllers;

/// <summary>
/// Controller for managing items in the SafeShare application.
/// This controller includes functionalities for item creation, modification, deletion, 
/// and retrieval of item details.
/// It communicates with the business logic layer using MediatR for a clean, decoupled architecture.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="ItemController"/>
/// </remarks>
/// <param name="mediator">The instance of mediator used to send commands and queries</param>
[Authorize(Roles = "User")]
[ServiceFilter(typeof(VerifyUser))]
public class ItemController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>
    /// Retrieves details of a specific item.
    /// </summary>
    /// <param name="itemId">The identifier of the item whose details are being requested.</param>
    /// <param name="userId">The identifier of the user requesting item details.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Detailed information about the specified item.</returns>
    [HttpGet(Route_ItemRoutes.GetItemDetails)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_Item>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_Item>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_Item>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    GetItemDetails
    (
        [FromRoute] Guid itemId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetItemByIdQuery(itemId, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Creates a new item.
    /// </summary>
    /// <param name="userId">The identifier of the owner creating the item.</param>
    /// <param name="createItemDto">The details of the item to be created.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A boolean value indicating whether the item was successfully created.</returns>
    [HttpPost(Route_ItemRoutes.CreateItem)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_Item>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    CreateItem
    (
        [FromRoute] Guid userId,
        [FromForm] DTO_CreateItem createItemDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new Mediatr_CreateItemCommand(userId.ToString(), createItemDto), cancellationToken);
    }

    /// <summary>
    /// Edits the details of an existing item.
    /// </summary>
    /// <param name="itemId">The identifier of the item to be edited.</param>
    /// <param name="userId">The identifier of the user editing the item.</param>
    /// <param name="editItemDto">The new details to update the item with.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A boolean value indicating whether the item was successfully edited.</returns>
    [HttpPut(Route_ItemRoutes.EditItem)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_Item>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_Item>>>
    EditItem
    (
        [FromRoute] Guid itemId,
        [FromRoute] Guid userId,
        [FromForm] DTO_UpdateItem editItemDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_EditItemCommand(itemId, userId.ToString(), editItemDto), cancellationToken);
    }

    /// <summary>
    /// Deletes an existing item.
    /// </summary>
    /// <param name="itemId">The identifier of the item to be deleted.</param>
    /// <param name="userId">The identifier of the user deleting the item.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A boolean value indicating whether the item was successfully deleted.</returns>
    [HttpDelete(Route_ItemRoutes.DeleteItem)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeleteItem
    (
        [FromRoute] Guid itemId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_DeleteItemCommand(itemId, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Retrieves all items for a specific shop.
    /// </summary>
    /// <param name="shopId">The identifier of the shop whose items are to be retrieved.</param>
    /// <param name="userId">The identifier of the user requesting the items.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A list of items associated with the shop.</returns>
    [HttpGet(Route_ItemRoutes.GetItemsByShopId)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_Item>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_Item>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_Item>>>>
    GetItemsByShopId
    (
        [FromRoute] Guid shopId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetItemsByShopIdQuery(shopId, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Shares an item with another user or publicly.
    /// </summary>
    /// <param name="shareItemDto">The details of the item sharing.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="userId">The id of the user making the request.</param>
    /// <returns>A boolean value indicating whether the item was successfully shared.</returns>
    [HttpPost(Route_ItemRoutes.ShareItem)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ShareItem
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_ShareItem shareItemDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ShareItemCommand(shareItemDto), cancellationToken);
    }

    /// <summary>
    /// Moderates an item, approving or rejecting it.
    /// </summary>
    /// <param name="moderateItemDto">The details of the moderation action.</param>
    /// <param name="userId">The identifier of the moderator.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A boolean value indicating whether the moderation action was successful.</returns>
    [Authorize(Roles = "Moderator")]
    [HttpPost(Route_ItemRoutes.ModerateItem)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    ModerateItem
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_ModerateItem moderateItemDto,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_ModerateItemCommand(moderateItemDto, userId.ToString()), cancellationToken);
    }

    /// <summary>
    /// Gets all items for moderation subject
    /// </summary>
    /// <param name="userId">The identifier of the moderator.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A boolean value indicating whether the items were retrieved succsessfully.</returns>
    [Authorize(Roles = "Moderator")]
    [HttpPost(Route_ItemRoutes.GetItemsForModeration)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<IEnumerable<DTO_Item>>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<IEnumerable<DTO_Item>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<IEnumerable<DTO_Item>>))]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Item>>>>
    GetItemsSubjectForModeration
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_GetItemsSubjectForModerationQuery(userId), cancellationToken);
    }

    /// <summary>
    /// Retrieves all items for a user.
    /// </summary>
    /// <param name="userId">The identifier of the user whose items are to be retrieved.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>A list of items associated with the user.</returns>
    [HttpGet(Route_ItemRoutes.GetUserItems)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<IEnumerable<DTO_Item>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<IEnumerable<DTO_Item>>))]
    public async Task<ActionResult<Util_GenericResponse<IEnumerable<DTO_Item>>>>
    GetUserItems
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        return await _mediator.Send(new MediatR_GetUserItemsQuery(userId.ToString()), cancellationToken);
    }
}