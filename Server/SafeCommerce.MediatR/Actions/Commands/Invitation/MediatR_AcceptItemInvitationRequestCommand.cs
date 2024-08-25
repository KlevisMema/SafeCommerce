using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Invitation;

public class MediatR_AcceptItemInvitationRequestCommand
(
    DTO_InvitationItemRequestActions dTO_AcceptInvitationRequest
) : IRequest<ObjectResult>
{
    public DTO_InvitationItemRequestActions DTO_AcceptInvitationRequest { get; set; } = dTO_AcceptInvitationRequest;
}