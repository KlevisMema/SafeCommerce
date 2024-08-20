using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Item;

public class Mediatr_CreateItemCommand
(
    string userId,
    DTO_CreateItem createItemDto
) : IRequest<ObjectResult>
{
    public string UserId { get; } = userId;
    public DTO_CreateItem CreateItemDto { get; } = createItemDto;
}