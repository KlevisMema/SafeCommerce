using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_RemoveUserFromItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_RemoveUserFromItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_RemoveUserFromItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var removeUserResult = await _service.RemoveUserFromIem(request.OwnerId, request.DTO_RemoveUserFromItem, cancellationToken);

        return Util_GenericControllerResponse<bool>.ControllerResponse(removeUserResult);
    }
}