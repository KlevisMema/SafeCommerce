using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_CreateItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<Mediatr_CreateItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        Mediatr_CreateItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.CreateItem(request.CreateItemDto, request.UserId, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}