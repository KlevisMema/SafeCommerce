/* 
 * Defines a MediatR command handler for processing account activation confirmation requests.
 * This handler is responsible for invoking the account management service to confirm the activation of a user's account based on the provided confirmation data.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// A MediatR command handler for processing account activation confirmation requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ActivateAccountConfirmationRequestCommandHandler"/> class.
/// </remarks>
/// <param name="service">The account management service used for confirming account activations.</param>
public class MediatR_ActivateAccountConfirmationRequestCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_ActivateAccountConfirmationRequestCommand, ObjectResult>
{
    /// <summary>
    /// Handles the process of confirming a user's account activation.
    /// </summary>
    /// <param name="request">The command containing the details for the account activation confirmation.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult with the outcome of the account activation confirmation process.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_ActivateAccountConfirmationRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        var confirmAccountActivationResult = await _service.ActivateAccountConfirmation(request.DTO_ActivateAccount);

        return Util_GenericControllerResponse<bool>.ControllerResponse(confirmAccountActivationResult);
    }
}