using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_RejectInvitationRequestCommandHandler
(
    IShopInvitations service
) : MediatR_GenericHandler<IShopInvitations>(service),
    IRequestHandler<MediatR_RejectInvitationRequestCommand, ObjectResult>

{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_RejectInvitationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var rejectInvitationRequest = await _service.RejectInvitation(request.RejectInvitationRequest);

        return Util_GenericControllerResponse<bool>.ControllerResponse(rejectInvitationRequest);
    }
}