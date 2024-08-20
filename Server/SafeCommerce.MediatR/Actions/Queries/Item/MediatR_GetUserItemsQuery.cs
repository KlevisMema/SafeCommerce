using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Item;
public class MediatR_GetUserItemsQuery
(
    string userId
) : IRequest<ObjectResult>
{
    public string UserId { get; } = userId;
}