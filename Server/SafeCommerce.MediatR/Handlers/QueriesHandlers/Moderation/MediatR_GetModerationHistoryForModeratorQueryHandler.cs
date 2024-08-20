using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Queries.Moderation;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Moderation;

public class MediatR_GetModerationHistoryForModeratorQueryHandler
(
    IModerationService service
) : MediatR_GenericHandler<IModerationService>(service),
    IRequestHandler<MediatR_GetModerationHistoryForModeratorQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetModerationHistoryForModeratorQuery request,
        CancellationToken cancellationToken
    )
    {
        var moderationHistory = await _service.GetModerationHistoryForModerator(request.ModeratorId, cancellationToken);

        return Util_GenericControllerResponse<DTO_SplittedModerationHistory>.ControllerResponse(moderationHistory);
    }
}