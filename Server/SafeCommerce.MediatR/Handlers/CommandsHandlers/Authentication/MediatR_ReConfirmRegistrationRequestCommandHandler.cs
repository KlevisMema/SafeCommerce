/* 
 * Defines a MediatR command handler for processing requests to re-send user registration confirmation emails.
 * This handler is responsible for invoking the appropriate authentication service to handle the reconfirmation of the registration process, typically triggered when a user did not receive or respond to the initial confirmation email.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

/// <summary>
/// A MediatR command handler for processing requests to re-send user registration confirmation emails.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ReConfirmRegistrationRequestCommandHandler"/> class.
/// </remarks>
/// <param name="service">The authentication service used for re-confirming user registration requests.</param>
public class MediatR_ReConfirmRegistrationRequestCommandHandler
(
    IAUTH_Register service
) : MediatR_GenericHandler<IAUTH_Register>(service),
    IRequestHandler<MediatR_ReConfirmRegistrationRequestCommand, ObjectResult>
{
    /// <summary>
    /// Handles the reconfirmation of a user registration request.
    /// </summary>
    /// <param name="request">The command containing the user's email for reconfirmation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult with the outcome of the reconfirmation process.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_ReConfirmRegistrationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var reregistrationRequestResult = await _service.ReConfirmRegistrationRequest(request.ReConfirmRegistration.Email);

        return Util_GenericControllerResponse<bool>.ControllerResponse(reregistrationRequestResult);
    }
}