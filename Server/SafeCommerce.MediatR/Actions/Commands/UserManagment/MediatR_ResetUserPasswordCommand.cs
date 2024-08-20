/* 
 * Defines a MediatR command for processing a user's password reset request.
 * This command is intended for use within MediatR handlers to handle the process of resetting a user's password based on the provided reset password details.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command for resetting a user's password.
/// This command includes the necessary data for processing a password reset request.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ResetUserPasswordCommand"/> class.
/// </remarks>
/// <param name="resetPassword">The DTO containing data for the password reset request.</param>
public class MediatR_ResetUserPasswordCommand
(
    DTO_ResetPassword resetPassword
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Data transfer object containing the information required for resetting a user's password.
    /// </summary>  
    public DTO_ResetPassword ResetPassword { get; set; } = resetPassword;
}