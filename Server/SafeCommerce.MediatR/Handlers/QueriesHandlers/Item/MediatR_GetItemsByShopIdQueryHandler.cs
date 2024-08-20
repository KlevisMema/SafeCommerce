using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;

public class MediatR_GetItemsByShopIdQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetItemsByShopIdQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetItemsByShopIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var items = await _service.GetItemsByShopId(request.ShopId, request.UserId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_Item>>.ControllerResponse(items);
    }
}