using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Item;
public class MediatR_EditItemCommand
(
    Guid itemId,
    string userId,
    DTO_UpdateItem updateItemDto
) : IRequest<ObjectResult>
{
    public Guid ItemId { get; } = itemId;
    public string UserId { get; } = userId;
    public DTO_UpdateItem UpdateItemDto { get; } = updateItemDto;
}