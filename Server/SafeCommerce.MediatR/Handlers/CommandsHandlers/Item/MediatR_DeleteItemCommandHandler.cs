using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_DeleteItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_DeleteItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_DeleteItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.DeleteItem(request.ItemId, request.UserId, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}