using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Commands.Item;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Item;

public class MediatR_EditItemCommandHandler
(
    IItemService service
) : MediatR_GenericHandler<IItemService>(service),
    IRequestHandler<MediatR_EditItemCommand, ObjectResult>
{
    public async Task<ObjectResult> Handle
    (
        MediatR_EditItemCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _service.UpdateItem(request.ItemId, request.UpdateItemDto, request.UserId, cancellationToken);

        return Util_GenericControllerResponse<DTO_Item>.ControllerResponse(result);
    }
}