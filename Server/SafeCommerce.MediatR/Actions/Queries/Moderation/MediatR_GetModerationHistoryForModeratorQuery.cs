using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Moderation;
public class MediatR_GetModerationHistoryForModeratorQuery(Guid moderatorId) : IRequest<ObjectResult>
{
    public Guid ModeratorId { get; } = moderatorId;
}