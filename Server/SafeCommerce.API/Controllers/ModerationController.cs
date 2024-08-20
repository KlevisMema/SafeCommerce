using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using Microsoft.AspNetCore.Authorization;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.Security.API.ActionFilters;
using SafeCommerce.MediatR.Actions.Queries.Moderation;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.API.Controllers;
public class ModerationController(IMediator mediator) : BaseController(mediator)
{
    [Authorize(Roles = "Moderator")]
    [ServiceFilter(typeof(VerifyUser))]
    [HttpGet(Route_Moderation.GetModerationHistoryForModerator)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Util_GenericResponse<DTO_SplittedModerationHistory>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Util_GenericResponse<DTO_SplittedModerationHistory>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Util_GenericResponse<DTO_SplittedModerationHistory>))]
    public async Task<ActionResult<Util_GenericResponse<DTO_SplittedModerationHistory>>>
    GetModerationHistoryForModerator
    (
        [FromRoute] Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _mediator.Send(new MediatR_GetModerationHistoryForModeratorQuery(userId), cancellationToken);
    }
}