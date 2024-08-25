using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.API.ActionFilters;
using SafeCommerce.API.Helpers.AttributeFilters;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.MediatR.Actions.Queries.Invitation;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.API.Controllers;

[ApiController]
[Authorize(Roles = "User")]
[Route("api/[controller]")]
[ServiceFilter(typeof(VerifyUser))]
public class InvitationController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet(Route_InvitationRoute.GetShopsInvitations)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_RecivedInvitations>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_RecivedInvitations>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_RecivedInvitations>>>>
    GetShopsInvitations
    (
        [FromRoute] Guid userId
    )
    {
        return await _mediator.Send(new MediatR_GetShopsInvitationsQuery(userId));
    }

    [HttpGet(Route_InvitationRoute.GetSentShopInvitations)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_SentInvitations>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_SentInvitations>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_SentInvitations>>>>
    GetSentShopInvitations
    (
        [FromRoute] Guid userId
    )
    {
        return await _mediator.Send(new MediatR_GetSentShopInvitationsQuery(userId));
    }

    [HttpPost(Route_InvitationRoute.SendInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    SendInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_SendInvitationRequest dTO_SendInvitation
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_SendInvitationCommand(dTO_SendInvitation));
    }

    [HttpPost(Route_InvitationRoute.AcceptInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    AcceptInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationRequestActions acceptInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_AcceptInvitationRequestCommand(acceptInvitationRequest));
    }

    [HttpPost(Route_InvitationRoute.RejectInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RejectInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationRequestActions rejectInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_RejectInvitationRequestCommand(rejectInvitationRequest));
    }

    [HttpDelete(Route_InvitationRoute.DeleteInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeleteInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationRequestActions deleteInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_DeleteSentInvitationCommand(deleteInvitationRequest));
    }

    [HttpGet(Route_InvitationRoute.GetItemsInvitations)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_RecivedItemInvitation>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_RecivedItemInvitation>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_RecivedItemInvitation>>>>
    GetItemsInvitations
    (
        [FromRoute] Guid userId
    ) 
    {
        return await _mediator.Send(new MediatR_GetItemsInvitationsQuery(userId));
    }

    [HttpGet(Route_InvitationRoute.GetSentItemsInvitations)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<List<DTO_SentItemInvitation>>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<List<DTO_SentItemInvitation>>))]
    public async Task<ActionResult<Util_GenericResponse<List<DTO_SentItemInvitation>>>>
    GetSentItemInvitations
    (
        [FromRoute] Guid userId
    )
    {
        return await _mediator.Send(new MediatR_GetSentItemInvitationsQuery(userId));
    }

    [HttpPost(Route_InvitationRoute.SendItemInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    SendItemInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_SendItemInvitationRequest dTO_SendInvitation
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_SendItemInvitationCommand(dTO_SendInvitation));
    }

    [HttpPost(Route_InvitationRoute.AcceptItemInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    AcceptItemInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationItemRequestActions acceptInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_AcceptItemInvitationRequestCommand(acceptInvitationRequest));
    }

    [HttpPost(Route_InvitationRoute.RejectItemInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    RejectItemInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationItemRequestActions rejectInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_RejectItemInvitationRequestCommand(rejectInvitationRequest));
    }

    [HttpDelete(Route_InvitationRoute.DeleteItemInvitation)]
    [ServiceFilter(typeof(API_Helper_AntiforgeryValidationFilter))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Util_GenericResponse<bool>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<bool>))]
    public async Task<ActionResult<Util_GenericResponse<bool>>>
    DeleteItemInvitation
    (
        [FromRoute] Guid userId,
        [FromBody] DTO_InvitationItemRequestActions deleteInvitationRequest
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return await _mediator.Send(new MediatR_DeleteSentItemInvitationCommand(deleteInvitationRequest));
    }
}