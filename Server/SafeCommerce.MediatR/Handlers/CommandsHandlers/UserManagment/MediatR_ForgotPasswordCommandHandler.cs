/* 
 * Defines a MediatR command handler for processing forgotten password requests.
 * This handler is responsible for invoking the account management service to handle requests for resetting passwords, typically involving sending a reset link or token to the user's email address.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// A MediatR command handler for processing forgotten password requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ForgotPasswordCommandHandler"/> class.
/// </remarks>
/// <param name="service">The account management service used for handling forgotten password requests.</param>
public class MediatR_ForgotPasswordCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_ForgotPasswordCommand, ObjectResult>
{
    /// <summary>
    /// Handles the process of responding to a forgotten password request.
    /// </summary>
    /// <param name="request">The command containing the email address associated with the forgotten password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult indicating the success or failure of the forgotten password request processing.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_ForgotPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var forgotPasswordResult = await _service.ForgotPassword(request.Email);

        return Util_GenericControllerResponse<bool>.ControllerResponse(forgotPasswordResult);
    }
}