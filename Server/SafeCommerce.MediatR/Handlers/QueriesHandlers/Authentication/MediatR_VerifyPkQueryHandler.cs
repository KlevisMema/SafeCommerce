using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.MediatR.Actions.Queries.Authentication;

namespace SafeCommerce.MediatR.Handlers.QueriesHandlers.Authentication;
public class MediatR_VerifyPkQueryHandler
(
    IAUTH_Login loginUser
) : IRequestHandler<MediatR_VerifyPkQuery, Util_GenericResponse<bool>>
{
    public Task<Util_GenericResponse<bool>> Handle
    (
        MediatR_VerifyPkQuery request, 
        CancellationToken cancellationToken
    )
    {
        var result = loginUser.VerifyPk(request.UserId, request.PublicKey);

        return result;
    }
}