using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Moderation;

namespace SafeCommerce.MediatR.Actions.Commands.Item;
public class MediatR_ModerateItemCommand
(
    DTO_ModerateItem moderateItemDto, 
    string moderatorId
) : IRequest<ObjectResult>
{
    public DTO_ModerateItem ModerateItemDto { get; } = moderateItemDto;
    public string ModeratorId { get; } = moderatorId;
}