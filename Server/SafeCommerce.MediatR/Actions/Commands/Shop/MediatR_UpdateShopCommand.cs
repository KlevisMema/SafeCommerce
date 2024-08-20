using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.MediatR.Actions.Commands.Shop;

public class MediatR_UpdateShopCommand
(
    Guid shopId,
    DTO_UpdateShop updateShopDto,
    string ownerId
) : IRequest<ObjectResult>
{
    public Guid ShopId { get; } = shopId;
    public DTO_UpdateShop UpdateShopDto { get; } = updateShopDto;
    public string OwnerId { get; } = ownerId;
}