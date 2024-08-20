using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.MediatR.Actions.Queries.Invitation;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Invitation;

public class MediatR_GetShopsInvitationsQueryHandler
(
    IShopInvitations service
) : MediatR_GenericHandler<IShopInvitations>(service),
    IRequestHandler<MediatR_GetShopsInvitationsQuery, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_GetShopsInvitationsQuery request,
        CancellationToken cancellationToken
    )
    {
        var getGroupsInvitationsResult = await _service.GetRecivedShopsInvitations(request.UserId);

        return Util_GenericControllerResponse<DTO_RecivedInvitations>.ControllerResponseList(getGroupsInvitationsResult);
    }
}