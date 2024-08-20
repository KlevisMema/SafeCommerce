using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.MediatR.Actions.Commands.Shop;
public class MediatR_ModerateShopCommand(DTO_ModerateShop moderateShop, Guid moderatorId) : IRequest<ObjectResult>
{
    public DTO_ModerateShop ModerateShop { get; } = moderateShop;
    public Guid ModeratorId { get; } = moderatorId;
}