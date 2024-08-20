using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_DeleteSentInvitationCommand
(
    DTO_InvitationRequestActions deleteInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationRequestActions DeleteInvitationRequest { get; set; } = deleteInvitationRequest;
}