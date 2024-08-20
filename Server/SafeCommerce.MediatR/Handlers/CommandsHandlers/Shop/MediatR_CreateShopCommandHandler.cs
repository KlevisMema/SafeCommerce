using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;

public class MediatR_CreateShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_CreateShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_CreateShopCommand request, 
        CancellationToken cancellationToken
    )
    {
        var createdShop = await _service.CreateShop(request.CreateShop, request.OwnerId, cancellationToken);

        return Util_GenericControllerResponse<DTO_Shop>.ControllerResponse(createdShop);
    }
}