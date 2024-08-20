/*
 * Represents a MediatR command for registering a user.
 * This command is expected to return an ObjectResult upon execution.
*/

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for registering a user.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_RegisterUserCommand"/> class.
/// </remarks>
/// <param name="register">The registration details for the user.</param>
public class MediatR_RegisterUserCommand
(
    DTO_Register register
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Gets or sets the registration details for the user.
    /// </summary>
    public DTO_Register Register { get; set; } = register;
}