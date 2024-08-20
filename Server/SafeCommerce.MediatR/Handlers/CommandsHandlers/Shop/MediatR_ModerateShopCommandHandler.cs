using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;

public class MediatR_ModerateShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_ModerateShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_ModerateShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var moderatedShop = await _service.ModerateShop(request.ModeratorId, request.ModerateShop, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(moderatedShop);
    }
}