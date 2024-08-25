using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_RejectItemInvitationRequestCommandHandler
(
    IItemInvitations service
) : MediatR_GenericHandler<IItemInvitations>(service),
    IRequestHandler<MediatR_RejectItemInvitationRequestCommand, ObjectResult>

{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_RejectItemInvitationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var rejectInvitationRequest = await _service.RejectInvitation(request.RejectInvitationRequest);

        return Util_GenericControllerResponse<bool>.ControllerResponse(rejectInvitationRequest);
    }
}