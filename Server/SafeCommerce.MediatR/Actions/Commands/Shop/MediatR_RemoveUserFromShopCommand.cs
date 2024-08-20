using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.MediatR.Actions.Commands.Shop;

public class MediatR_RemoveUserFromShopCommand
(
    Guid ownerId,
    DTO_RemoveUserFromShop dTORemoveUserFromShop
) : IRequest<ObjectResult>
{
    public Guid OwnerId { get; } = ownerId;
    public DTO_RemoveUserFromShop DTORemoveUserFromShop { get; } = dTORemoveUserFromShop;
}