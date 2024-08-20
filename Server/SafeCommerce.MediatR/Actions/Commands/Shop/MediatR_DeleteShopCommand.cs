using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Commands.Shop;

public class MediatR_DeleteShopCommand
(
    Guid shopId,
    string ownerId
) : IRequest<ObjectResult>
{
    public Guid ShopId { get; } = shopId;
    public string OwnerId { get; } = ownerId;
}