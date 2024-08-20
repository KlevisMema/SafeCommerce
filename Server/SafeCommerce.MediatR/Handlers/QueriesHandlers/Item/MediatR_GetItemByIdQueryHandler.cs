using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;
public class MediatR_GetItemByIdQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetItemByIdQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetItemByIdQuery request, 
        CancellationToken cancellationToken
    )
    {
        var item = await _service.GetItemById(request.ItemId, request.UserId, cancellationToken);

        return Util_GenericControllerResponse<DTO_Item>.ControllerResponse(item);
    }
}