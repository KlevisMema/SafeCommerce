/* 
 * Defines a MediatR command for handling the re-confirmation of user registration.
 * This command is designed to be processed by MediatR handlers to resend registration confirmation requests, typically used when the initial confirmation was not completed.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for re-sending a user registration confirmation request.
/// This command includes the user's email to facilitate the re-confirmation process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ReConfirmRegistrationRequestCommand"/> class.
/// </remarks>
/// <param name="reConfirmRegistration">The email address of the user requesting re-confirmation.</param>
public class MediatR_ReConfirmRegistrationRequestCommand
(
    DTO_ReConfirmRegistration reConfirmRegistration
) : IRequest<ObjectResult>
{
    /// <summary>
    /// The email address of the user requesting re-confirmation of registration.
    /// </summary> 
    public DTO_ReConfirmRegistration ReConfirmRegistration { get; set; } = reConfirmRegistration;
}