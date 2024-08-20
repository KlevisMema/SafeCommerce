/* 
 * Defines a MediatR command handler for processing user logout requests.
 * This handler is responsible for invoking the appropriate authentication service to handle the logout process based on the user's ID.
 */

using MediatR;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

/// <summary>
/// A MediatR command handler for processing user logout requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_LogOutCommandHandler"/> class.
/// </remarks>
/// <param name="service">The authentication service used for logging out users.</param>
public class MediatR_LogOutCommandHandler
(
    IAUTH_Login service
) : MediatR_GenericHandler<IAUTH_Login>(service),
    IRequestHandler<MediatR_LogOutCommand>
{
    /// <summary>
    /// Handles the user logout process.
    /// </summary>
    /// <param name="request">The command containing the user's ID for logout.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous logout operation.</returns>
    public async Task
    Handle
    (
        MediatR_LogOutCommand request,
        CancellationToken cancellationToken
    )
    {
        await _service.LogOut(request.Id);
    }
}