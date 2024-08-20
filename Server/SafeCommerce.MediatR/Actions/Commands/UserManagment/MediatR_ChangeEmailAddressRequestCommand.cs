/* 
 * Defines a MediatR command for handling user email address change requests.
 * This command is used within MediatR handlers to facilitate the process of updating a user's email address, based on the provided user ID and new email details.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for processing a request to change a user's email address.
/// This command includes the user's identifier and the new email address details.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ChangeEmailAddressRequestCommand"/> class.
/// </remarks>
/// <param name="userId">The unique identifier of the user requesting the email address change.</param>
/// <param name="emailAddress">The DTO containing the new email address details.</param>
public class MediatR_ChangeEmailAddressRequestCommand
(
    Guid userId,
    DTO_ChangeEmailAddressRequest emailAddress
) : IRequest<ObjectResult>
{
    /// <summary>
    /// The unique identifier of the user requesting the email address change.
    /// </summary>
    public Guid UserId { get; set; } = userId;
    /// <summary>
    /// Data transfer object containing the new email address details.
    /// </summary>
    public DTO_ChangeEmailAddressRequest EmailAddress { get; set; } = emailAddress;
}