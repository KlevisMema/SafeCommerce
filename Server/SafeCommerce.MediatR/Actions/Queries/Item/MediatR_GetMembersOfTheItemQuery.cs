using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Item;

public class MediatR_GetMembersOfTheItemQuery
(
    Guid itemId,
    Guid ownerId
) : IRequest<ObjectResult>
{
    public Guid ItemId { get; } = itemId;
    public Guid OwnerId { get; } = ownerId;
}