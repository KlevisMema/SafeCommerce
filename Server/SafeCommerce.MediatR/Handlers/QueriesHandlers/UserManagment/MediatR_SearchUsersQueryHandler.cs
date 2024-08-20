using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.MediatR.Actions.Queries.UserManagment;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.UserManagment;

public class MediatR_SearchUsersQueryHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_SearchUsersQuery, ObjectResult>
{
    public async Task<ObjectResult>
    Handle
    (
        MediatR_SearchUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var getUserSearchedResult = await _service.SearchUserByUserName(request.UserName, request.UserId.ToString());

        return Util_GenericControllerResponse<DTO_UserSearched>.ControllerResponseList(getUserSearchedResult);
    }
}