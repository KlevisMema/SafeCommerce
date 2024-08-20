using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Item;
public class MediatR_GetItemByIdQuery
(
    Guid itemId,
    string userId
) : IRequest<ObjectResult>
{
    public Guid ItemId { get; } = itemId;
    public string UserId { get; } = userId;
}