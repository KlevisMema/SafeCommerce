using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;

public class MediatR_GetPublicSharedItemsQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetPublicSharedItemsQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetPublicSharedItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        var item = await _service.GetPublicSharedItems(request.UserId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_PublicItem>>.ControllerResponse(item);
    }
}