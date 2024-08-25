using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_DeleteSentItemInvitationCommandHandler
(
    IItemInvitations service
) : MediatR_GenericHandler<IItemInvitations>(service),
    IRequestHandler<MediatR_DeleteSentItemInvitationCommand, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_DeleteSentItemInvitationCommand request,
        CancellationToken cancellationToken
    )
    {
        var deleteInvitationResult = await _service.DeleteSentInvitation(request.DeleteInvitationRequest);

        return Util_GenericControllerResponse<bool>.ControllerResponse(deleteInvitationResult);
    }
}