using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Invitation;

public class MediatR_GetSentItemInvitationsQuery
(
    Guid userId
) : IRequest<ObjectResult>
{

    public Guid UserId { get; set; } = userId;
}