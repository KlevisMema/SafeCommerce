using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_DeleteSentItemInvitationCommand
(
    DTO_InvitationItemRequestActions deleteInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationItemRequestActions DeleteInvitationRequest { get; set; } = deleteInvitationRequest;
}