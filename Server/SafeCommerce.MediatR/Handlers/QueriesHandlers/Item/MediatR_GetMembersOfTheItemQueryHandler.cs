using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Item;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Item;

public class MediatR_GetMembersOfTheItemQueryHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_GetMembersOfTheItemQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetMembersOfTheItemQuery request,
        CancellationToken cancellationToken
    )
    {
        var item = await _service.GetMembersOfTheItem(request.ItemId, request.OwnerId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_ItemMembers>>.ControllerResponse(item);
    }
}