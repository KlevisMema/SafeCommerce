using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;

public class MediatR_DeleteShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_DeleteShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_DeleteShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var shopDeleted = await _service.DeleteShop(request.ShopId, request.OwnerId, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(shopDeleted);
    }
}