using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.MediatR.Actions.Queries.Shop;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Shop;

public class MediatR_GetMembersOfTheShopQueryHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_GetMembersOfTheShopQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (

        MediatR_GetMembersOfTheShopQuery request,
        CancellationToken cancellationToken
    )
    {
        var shopMembers = await _service.GetMembersOfTheShop(request.ShopId, request.OwnerId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_ShopMembers>>.ControllerResponse(shopMembers);
    }
}