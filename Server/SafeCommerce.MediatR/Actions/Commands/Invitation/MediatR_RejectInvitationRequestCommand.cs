using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_RejectInvitationRequestCommand
(
    DTO_InvitationRequestActions rejectInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationRequestActions RejectInvitationRequest { get; set; } = rejectInvitationRequest;
}