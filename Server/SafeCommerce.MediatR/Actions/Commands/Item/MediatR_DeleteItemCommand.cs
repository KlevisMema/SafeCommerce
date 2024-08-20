using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Commands.Item;

public class MediatR_DeleteItemCommand
(
    Guid itemId,
    string userId
) : IRequest<ObjectResult>
{
    public Guid ItemId { get; } = itemId;
    public string UserId { get; } = userId;
}