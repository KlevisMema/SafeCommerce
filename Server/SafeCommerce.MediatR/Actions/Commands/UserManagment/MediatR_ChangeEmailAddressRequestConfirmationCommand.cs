/* 
 * Defines a MediatR command for handling the confirmation of a user's email address change request.
 * This command is used within MediatR handlers to manage the confirmation process based on the provided user ID and email address confirmation details.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for confirming a request to change a user's email address.
/// This command includes the user's identifier and the details necessary for confirming the email address change.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ChangeEmailAddressRequestConfirmationCommand"/> class.
/// </remarks>
/// <param name="userId">The unique identifier of the user requesting the email address change confirmation.</param>
/// <param name="changeEmailAddressConfirm">The DTO containing the confirmation details for the email address change.</param>
public class MediatR_ChangeEmailAddressRequestConfirmationCommand
(
    Guid userId,
    DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirm
) : IRequest<Util_GenericResponse<DTO_Token>>
{
    /// <summary>
    /// The unique identifier of the user requesting confirmation of the email address change.
    /// </summary>
    public Guid UserId { get; set; } = userId;
    /// <summary>
    /// Data transfer object containing the confirmation details for the email address change.
    /// </summary>
    public DTO_ChangeEmailAddressRequestConfirm ChangeEmailAddressConfirm { get; set; } = changeEmailAddressConfirm;
}