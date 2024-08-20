/* 
 * Defines a MediatR command for initiating the account activation process.
 * This command is used within MediatR handlers to manage the request for activating a user account based on the provided email address.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.UserManagment.Interfaces;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for initiating an account activation request.
/// This command carries the email address associated with the account to be activated.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ActivateAccountRequestCommand"/> class.
/// </remarks>
/// <param name="email">The email address associated with the account to be activated.</param>
public class MediatR_ActivateAccountRequestCommand
(
    string email
) : IRequest<ObjectResult>
{
    /// <summary>
    /// The email address associated with the user account to be activated.
    /// </summary>
    public string Email { get; set; } = email;
}