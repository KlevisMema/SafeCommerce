/* 
 * Defines a MediatR command for processing account activation confirmation requests.
 * This command is intended to be handled by MediatR to activate a user's account based on the provided activation details.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for handling account activation confirmation.
/// This command includes the data necessary to confirm and activate a user's account.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ActivateAccountConfirmationRequestCommand"/> class.
/// </remarks>
/// <param name="dTO_ActivateAccount">The DTO containing account activation confirmation data.</param>
public class MediatR_ActivateAccountConfirmationRequestCommand
(
    DTO_ActivateAccountConfirmation dTO_ActivateAccount
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Data transfer object containing the information required for account activation confirmation.
    /// </summary>
    public DTO_ActivateAccountConfirmation DTO_ActivateAccount { get; set; } = dTO_ActivateAccount;
}