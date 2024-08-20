using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;
public class MediatR_GetUserItemsQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetUserItemsQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetUserItemsQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.GetUserItems(request.UserId, cancellationToken);
        return Util_GenericControllerResponse<IEnumerable<DTO_Item>>.ControllerResponse(result);
    }
}