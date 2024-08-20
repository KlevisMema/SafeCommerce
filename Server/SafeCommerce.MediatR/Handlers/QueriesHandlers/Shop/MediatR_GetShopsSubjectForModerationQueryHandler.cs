using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.MediatR.Actions.Queries.Shop;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Shop;

public class MediatR_GetShopsSubjectForModerationQueryHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_GetShopsSubjectForModerationQuery, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_GetShopsSubjectForModerationQuery request,
        CancellationToken cancellationToken
    )
    {
        var shopsForModeration = await _service.GetShopsSubjectForModeration(request.ModeratorId, cancellationToken);

        return Util_GenericControllerResponse<IEnumerable<DTO_ShopForModeration>>.ControllerResponse(shopsForModeration);
    }
}
