using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_SendInvitationCommandHandler
(
    IShopInvitations service
) : MediatR_GenericHandler<IShopInvitations>(service),
    IRequestHandler<MediatR_SendInvitationCommand, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_SendInvitationCommand request,
        CancellationToken cancellationToken
    )
    {
        var sendInvitationResult = await _service.SendInvitation(request.DTO_SendInvitation);

        return Util_GenericControllerResponse<bool>.ControllerResponse(sendInvitationResult);
    }
}