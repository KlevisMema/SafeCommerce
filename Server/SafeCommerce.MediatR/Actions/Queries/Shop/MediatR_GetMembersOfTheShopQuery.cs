using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Shop;

public class MediatR_GetMembersOfTheShopQuery(Guid shopId, Guid ownerId) : IRequest<ObjectResult>
{
    public Guid ShopId { get; } = shopId;
    public Guid OwnerId { get; } = ownerId;
}