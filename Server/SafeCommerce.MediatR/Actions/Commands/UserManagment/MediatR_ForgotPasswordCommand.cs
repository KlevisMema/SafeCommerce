/* 
 * Defines a MediatR command for processing a user's forgot password request.
 * This command is used within MediatR handlers to handle the process of resetting a password, based on a user's provided email address.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for handling a forgot password request.
/// This command includes the user's email address to initiate the password reset process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ForgotPasswordCommand"/> class.
/// </remarks>
/// <param name="email">The email address associated with the user's account for the password reset.</param>
public class MediatR_ForgotPasswordCommand
(
    string email
) : IRequest<ObjectResult>
{
    /// <summary>
    /// The email address of the user who has forgotten their password.
    /// </summary>
    public string Email { get; set; } = email;
}