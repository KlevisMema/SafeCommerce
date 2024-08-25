using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_AcceptItemInvitationRequestCommandHandler
(
    IItemInvitations service
) : MediatR_GenericHandler<IItemInvitations>(service),
    IRequestHandler<MediatR_AcceptItemInvitationRequestCommand, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_AcceptItemInvitationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var acceptInvitationRequest = await _service.AcceptInvitation(request.DTO_AcceptInvitationRequest);

        return Util_GenericControllerResponse<bool>.ControllerResponse(acceptInvitationRequest);
    }
}