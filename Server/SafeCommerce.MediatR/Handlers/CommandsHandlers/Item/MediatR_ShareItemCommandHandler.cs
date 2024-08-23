using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_ShareItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_ShareItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_ShareItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.ShareItem(request.OwnerId, request.ShareItemDto, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}