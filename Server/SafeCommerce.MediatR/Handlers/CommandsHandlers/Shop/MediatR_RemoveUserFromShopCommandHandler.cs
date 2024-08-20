using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Shop;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Shop;


public class MediatR_RemoveUserFromShopCommandHandler
(
    IShopService service
) : MediatR_GenericHandler<IShopService>(service),
    IRequestHandler<MediatR_RemoveUserFromShopCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_RemoveUserFromShopCommand request,
        CancellationToken cancellationToken
    )
    {
        var removeUserResult = await _service.RemoveUserFromShop(request.OwnerId, request.DTORemoveUserFromShop, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(removeUserResult);
    }
}