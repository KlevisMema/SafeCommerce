/*
 * Handles the MediatR_RegisterUserCommand.
 * This handler receives a registration request and uses the provided IAUTH_Register service to register the user.
*/

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

/// <summary>
/// Handles the MediatR_RegisterUserCommand.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_RegisterUserCommandHandler"/> class.
/// </remarks>
/// <param name="registerUser">The IAUTH_Register service used to register users.</param>
public class MediatR_RegisterUserCommandHandler
(
    IAUTH_Register registerUser
) : IRequestHandler<MediatR_RegisterUserCommand, ObjectResult>
{
    /// <summary>
    /// Handles the registration request and returns an appropriate ObjectResult.
    /// </summary>
    /// <param name="request">The registration request to handle.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An ObjectResult that represents the result of the registration operation.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_RegisterUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var registerResult = await registerUser.RegisterUser(request.Register);

        return Util_GenericControllerResponse<bool>.ControllerResponse(registerResult);
    }
}