using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.MediatR.Actions.Commands.Shop
{
    public class MediatR_CreateShopCommand
    (
        DTO_CreateShop createShopDto,
        string ownerId
    ) : IRequest<ObjectResult>
    {
        public DTO_CreateShop CreateShop { get; } = createShopDto;
        public string OwnerId { get; } = ownerId;
    }
}