using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_SendItemInvitationCommandHandler
(
    IItemInvitations service
) : MediatR_GenericHandler<IItemInvitations>(service),
    IRequestHandler<MediatR_SendItemInvitationCommand, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_SendItemInvitationCommand request,
        CancellationToken cancellationToken
    )
    {
        var sendInvitationResult = await _service.SendInvitation(request.DTO_SendInvitation);

        return Util_GenericControllerResponse<bool>.ControllerResponse(sendInvitationResult);
    }
}