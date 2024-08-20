/* 
 * Defines a MediatR command for confirming a user's registration.
 * This command is intended for use within MediatR handlers to facilitate the process of user registration confirmation.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for confirming a user's registration.
/// This command carries the necessary data for the registration confirmation process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ConfirmUserRegistrationCommand"/> class.
/// </remarks>
/// <param name="confirmRegistration">The DTO containing registration confirmation data.</param>
public class MediatR_ConfirmUserRegistrationCommand
(
    DTO_ConfirmRegistration confirmRegistration
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Data transfer object containing the information required for confirming user registration.
    /// </summary>
    public DTO_ConfirmRegistration ConfirmRegistration { get; set; } = confirmRegistration;
}