/* 
 * Defines a MediatR command handler for processing requests to initiate the account activation process.
 * This handler is responsible for invoking the account management service to handle the initial phase of activating a user's account, typically involving sending an activation email based on the provided email address.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// A MediatR command handler for processing requests to initiate the account activation process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ActivateAccountRequestCommandHandler"/> class.
/// </remarks>
/// <param name="service">The account management service used for initiating account activations.</param>
public class MediatR_ActivateAccountRequestCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_ActivateAccountRequestCommand, ObjectResult>
{
    /// <summary>
    /// Handles the process of initiating the account activation.
    /// </summary>
    /// <param name="request">The command containing the email address for initiating the account activation process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult with the outcome of the account activation initiation process.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_ActivateAccountRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var activateAccountResult = await _service.ActivateAccountRequest(request.Email);

        return Util_GenericControllerResponse<bool>.ControllerResponse(activateAccountResult);
    }
}