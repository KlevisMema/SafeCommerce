using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.MediatR.Actions.Queries.Invitation;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Invitation;

public class MediatR_GetSentShopInvitationsQueryHandler
(
    IShopInvitations service
) : MediatR_GenericHandler<IShopInvitations>(service),
    IRequestHandler<MediatR_GetSentShopInvitationsQuery, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_GetSentShopInvitationsQuery request,
        CancellationToken cancellationToken
    )
    {
        var getSentInvitationsResult = await _service.GetSentGroupInvitations(request.UserId);

        return Util_GenericControllerResponse<DTO_SentInvitations>.ControllerResponseList(getSentInvitationsResult);
    }
}
