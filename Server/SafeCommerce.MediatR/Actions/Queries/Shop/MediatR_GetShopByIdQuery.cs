using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Shop;
public class MediatR_GetShopByIdQuery
(
    Guid shopId,
    string userId
) : IRequest<ObjectResult>
{
    public Guid ShopId { get; } = shopId;
    public string UserId { get; } = userId;
}