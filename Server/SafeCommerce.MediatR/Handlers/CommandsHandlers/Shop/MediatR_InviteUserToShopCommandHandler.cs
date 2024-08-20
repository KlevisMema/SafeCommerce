using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.Shop;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;
public class MediatR_InviteUserToShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_InviteUserToShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_InviteUserToShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.InviteUserToShop(request.InviteUserToShopDto, request.OwnerId, cancellationToken);
        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}