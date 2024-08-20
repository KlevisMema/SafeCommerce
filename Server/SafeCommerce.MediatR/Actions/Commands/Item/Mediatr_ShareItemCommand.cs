using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.MediatR.Actions.Commands.Item;

public class MediatR_ShareItemCommand
(
    DTO_ShareItem shareItemDto
) : IRequest<ObjectResult>
{
    public DTO_ShareItem ShareItemDto { get; } = shareItemDto;
}