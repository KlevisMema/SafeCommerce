using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;

public class MediatR_GetItemsSubjectForModerationQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetItemsSubjectForModerationQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetItemsSubjectForModerationQuery request,
        CancellationToken cancellationToken
    )
    {
        var itemsForModeration = await _service.GetItemsSubjectForModeration(request.ModeratorId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_Item>>.ControllerResponse(itemsForModeration);
    }
}