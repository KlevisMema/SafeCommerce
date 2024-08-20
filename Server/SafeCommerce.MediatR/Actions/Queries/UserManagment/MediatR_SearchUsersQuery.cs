using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.UserManagment;

public class MediatR_SearchUsersQuery
(
    Guid userId,
    string userName
) : IRequest<ObjectResult>
{
    public string UserName { get; set; } = userName;

    public Guid UserId { get; set; } = userId;
}