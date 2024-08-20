using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Item;
public class MediatR_GetItemsByShopIdQuery
(
    Guid shopId,
    string userId
) : IRequest<ObjectResult>
{
    public Guid ShopId { get; } = shopId;
    public string UserId { get; } = userId;
}