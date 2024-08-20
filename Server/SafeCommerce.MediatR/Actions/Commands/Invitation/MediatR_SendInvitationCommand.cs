using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_SendInvitationCommand
(
    DTO_SendInvitationRequest dTO_SendInvitation
) : IRequest<ObjectResult>
{
    public DTO_SendInvitationRequest DTO_SendInvitation { get; set; } = dTO_SendInvitation;
}