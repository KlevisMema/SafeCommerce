using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_ModerateItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_ModerateItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_ModerateItemCommand request, 
        CancellationToken cancellationToken
    )
    {
        var result = await _service.ModerateItem(request.ModerateItemDto, request.ModeratorId, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(result);
    }
}