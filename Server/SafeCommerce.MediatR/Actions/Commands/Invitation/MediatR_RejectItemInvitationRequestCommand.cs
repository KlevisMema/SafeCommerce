using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_RejectItemInvitationRequestCommand
(
    DTO_InvitationItemRequestActions rejectInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationItemRequestActions RejectInvitationRequest { get; set; } = rejectInvitationRequest;
}