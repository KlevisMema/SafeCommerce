/* 
 * Defines a MediatR command for handling the logout process of a user.
 * This command is used within the MediatR framework to initiate and process user logout actions.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for handling user logout.
/// This command carries the user identifier to process the logout action.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_LogOutCommand"/> class.
/// </remarks>
/// <param name="id">The identifier of the user who is logging out.</param>
public class MediatR_LogOutCommand
(
    string id
) : IRequest
{
    /// <summary>
    /// The identifier of the user who is logging out.
    /// </summary>
    public string Id { get; set; } = id;
}