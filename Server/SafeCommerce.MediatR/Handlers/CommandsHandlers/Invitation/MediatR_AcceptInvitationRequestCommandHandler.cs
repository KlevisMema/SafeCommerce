using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.MediatR.Actions.Commands.Invitation;
using SafeCommerce.BLL.Interfaces;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Invitation;

public class MediatR_AcceptInvitationRequestCommandHandler
(
    IShopInvitations service
) : MediatR_GenericHandler<IShopInvitations>(service),
    IRequestHandler<MediatR_AcceptInvitationRequestCommand, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_AcceptInvitationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var acceptInvitationRequest = await _service.AcceptInvitation(request.DTO_AcceptInvitationRequest);

        return Util_GenericControllerResponse<bool>.ControllerResponse(acceptInvitationRequest);
    }
}