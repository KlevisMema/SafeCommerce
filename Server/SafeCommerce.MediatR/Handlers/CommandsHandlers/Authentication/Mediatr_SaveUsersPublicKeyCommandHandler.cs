using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

public class Mediatr_SaveUsersPublicKeyCommandHandler
(
    IAUTH_Login loginUser
) : IRequestHandler<Mediatr_SaveUsersPublicKeyCommand, Util_GenericResponse<string>>
{
    public Task<Util_GenericResponse<string>> Handle
    (
        Mediatr_SaveUsersPublicKeyCommand request, 
        CancellationToken cancellationToken
    )
    {
        var result = loginUser.SaveUsersPublicKey(request.UserId, request.PublicKey);

        return result;
    }
}