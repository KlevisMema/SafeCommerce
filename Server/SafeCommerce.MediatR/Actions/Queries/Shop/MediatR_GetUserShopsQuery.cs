using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.Shop;
public class MediatR_GetUserShopsQuery
(
    string userId
) : IRequest<ObjectResult>
{
    public string UserId { get; } = userId;
}
