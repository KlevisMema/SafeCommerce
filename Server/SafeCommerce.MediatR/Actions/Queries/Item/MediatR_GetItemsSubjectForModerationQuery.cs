using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Item;

public class MediatR_GetItemsSubjectForModerationQuery
(
    Guid moderatorId
) : IRequest<ObjectResult>
{
    public Guid ModeratorId { get; } = moderatorId;
}