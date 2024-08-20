/* 
 * Defines a MediatR command handler for deactivating users.
*/

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// Represents a MediatR handler that processes commands to deactivate users.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_DeactivateAccountCommandHandler"/> class with the specified account management service.
/// </remarks>
/// <param name="service">The account management service.</param>
public class MediatR_DeactivateAccountCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_DeactivateAccountCommand, ObjectResult>
{

    /// <summary>
    /// Processes the provided command to deactivate a user.
    /// </summary>
    /// <param name="request">The deactivated user command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result indicating the success or failure of the user deactivation operation.</returns>
    public async Task<ObjectResult>
    Handle
    (
        MediatR_DeactivateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var deleteUserResult = await _service.DeactivateAccount(request.Id, request.DeactivateAccount);

        return Util_GenericControllerResponse<bool>.ControllerResponse(deleteUserResult);
    }
}