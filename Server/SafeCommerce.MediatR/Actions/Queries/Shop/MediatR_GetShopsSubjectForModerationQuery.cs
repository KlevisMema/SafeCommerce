using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Shop;

public class MediatR_GetShopsSubjectForModerationQuery(Guid moderatorId) : IRequest<ObjectResult>
{
    public Guid ModeratorId { get; } = moderatorId;
}