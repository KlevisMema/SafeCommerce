using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.MediatR.Actions.Queries.Invitation;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Invitation;

public class MediatR_GetItemsInvitationsQueryHandler
(
    IItemInvitations service
) : MediatR_GenericHandler<IItemInvitations>(service),
    IRequestHandler<MediatR_GetItemsInvitationsQuery, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_GetItemsInvitationsQuery request,
        CancellationToken cancellationToken
    )
    {
        var getItemsInvitationsResult = await _service.GetRecivedItemsInvitations(request.UserId);

        return Util_GenericControllerResponse<DTO_RecivedItemInvitation>.ControllerResponseList(getItemsInvitationsResult);
    }
}