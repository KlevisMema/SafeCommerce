using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;
public class MediatR_UpdateShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_UpdateShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_UpdateShopCommand request, 
        CancellationToken cancellationToken
    )
    {
        var updatedShop = await _service.UpdateShop(request.ShopId, request.UpdateShopDto, request.OwnerId, cancellationToken);

        return Util_GenericControllerResponse<DTO_Shop>.ControllerResponse(updatedShop);
    }
}