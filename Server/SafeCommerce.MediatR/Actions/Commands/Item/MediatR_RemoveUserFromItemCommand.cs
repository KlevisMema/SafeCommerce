using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Item;

public class MediatR_RemoveUserFromItemCommand
(
    Guid ownerId,
    DTO_RemoveUserFromItem dTO_RemoveUserFromItem
) : IRequest<ObjectResult>
{
    public Guid OwnerId { get; } = ownerId;
    public DTO_RemoveUserFromItem DTO_RemoveUserFromItem { get; } = dTO_RemoveUserFromItem;
}