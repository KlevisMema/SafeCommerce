/* 
 * Defines a MediatR command handler for confirming a user's login.
 * This handler processes the confirmation of user logins using an OTP (One-Time Password), utilizing the appropriate authentication service.
 */

using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

/// <summary>
/// A MediatR command handler for confirming a user's login using OTP (One-Time Password).
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ConfirmLoginUserCommandHandler"/> class.
/// </remarks>
/// <param name="service">The authentication service used for login confirmation.</param>
public class MediatR_ConfirmLoginUserCommandHandler(IAUTH_Login service) : 
    MediatR_GenericHandler<IAUTH_Login>(service),
    IRequestHandler<MediatR_ConfirmLoginUserCommand, Util_GenericResponse<DTO_LoginResult>>
{
    /// <summary>
    /// Handles the confirmation of a user login via OTP.
    /// </summary>
    /// <param name="request">The command containing user ID and OTP for confirmation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult with the outcome of the login confirmation process.</returns>
    public async Task<Util_GenericResponse<DTO_LoginResult>>
    Handle
    (
        MediatR_ConfirmLoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var confirmLoginResult = await _service.ConfirmLogin(request.UserId, request.OTP);

        return confirmLoginResult;
    }
}