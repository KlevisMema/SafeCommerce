using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_AcceptInvitationRequestCommand
(
    DTO_InvitationRequestActions dTO_AcceptInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationRequestActions DTO_AcceptInvitationRequest { get; set; } = dTO_AcceptInvitationRequest;
}