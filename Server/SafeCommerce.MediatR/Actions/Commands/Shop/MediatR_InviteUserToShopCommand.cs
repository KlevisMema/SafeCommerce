using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.MediatR.Actions.Commands.Shop;
public class MediatR_InviteUserToShopCommand
(
    DTO_InviteUserToShop inviteUserToShopDto,
    string ownerId
) : IRequest<ObjectResult>
{
    public DTO_InviteUserToShop InviteUserToShopDto { get; } = inviteUserToShopDto;
    public string OwnerId { get; } = ownerId;
}