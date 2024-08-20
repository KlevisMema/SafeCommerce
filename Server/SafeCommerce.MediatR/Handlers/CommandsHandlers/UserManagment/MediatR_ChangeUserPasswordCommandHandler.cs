/* 
 * Defines a MediatR command handler for changing user passwords.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// Represents a MediatR handler that processes commands to change user passwords.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ChangeUserPasswordCommandHandler"/> class with the specified account management service.
/// </remarks>
/// <param name="service">The account management service.</param>
public class MediatR_ChangeUserPasswordCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_ChangeUserPasswordCommand, ObjectResult>
{

    /// <summary>
    /// Processes the provided command to change user passwords.
    /// </summary>
    /// <param name="request">The change password command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result indicating the success or failure of the password change operation.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_ChangeUserPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        var changePasswordResult = await _service.ChangePassword(request.Id, request.UpdatePasswordDto);

        return Util_GenericControllerResponse<bool>.ControllerResponse(changePasswordResult);
    }
}