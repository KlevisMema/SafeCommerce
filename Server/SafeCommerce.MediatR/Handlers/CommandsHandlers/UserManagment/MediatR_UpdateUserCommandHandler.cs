/* 
 * Defines a MediatR command handler for updating user information.
 */

using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.MediatR.Actions.Commands.UserManagment;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.UserManagment;

/// <summary>
/// Represents a MediatR handler that processes commands to update user information.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_UpdateUserCommandHandler"/> class with the specified account management service.
/// </remarks>
/// <param name="service">The account management service.</param>
public class MediatR_UpdateUserCommandHandler
(
    IAccountManagment service
) : MediatR_GenericHandler<IAccountManagment>(service),
    IRequestHandler<MediatR_UpdateUserCommand, Util_GenericResponse<DTO_UserUpdatedInfo>>
{

    /// <summary>
    /// Processes the provided command to update user information.
    /// </summary>
    /// <param name="request">The update user command request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result indicating the success or failure of the user update operation.</returns>
    public async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    Handle
    (
        MediatR_UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var updateUserResult = await _service.UpdateUser(request.Id, request.DTO_UserInfo);

        return updateUserResult;
    }
}