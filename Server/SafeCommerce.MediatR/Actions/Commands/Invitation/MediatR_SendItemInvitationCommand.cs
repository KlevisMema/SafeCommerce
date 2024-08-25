using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_SendItemInvitationCommand
(
    DTO_SendItemInvitationRequest dTO_SendInvitation
) : IRequest<ObjectResult>
{
    public DTO_SendItemInvitationRequest DTO_SendInvitation { get; set; } = dTO_SendInvitation;
}