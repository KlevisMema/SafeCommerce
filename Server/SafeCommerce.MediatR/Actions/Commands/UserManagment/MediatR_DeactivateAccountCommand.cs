/* 
 * Defines a MediatR command for deactivating a user.
 */

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command to delete a user based on their ID.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_DeactivateAccountCommand"/> class with the specified user ID.
/// </remarks>
/// <param name="id">The ID of the user to be deactivated.</param>
public class MediatR_DeactivateAccountCommand
(
    Guid id,
    DTO_DeactivateAccount deactivateAccount
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Gets or sets the ID of the user to be deactivated.
    /// </summary>
    public Guid Id { get; set; } = id;
    /// <summary>
    /// Gets or sets the <see cref="DTO_DeactivateAccount"/> object of the user to be deactivated.
    /// </summary>
    public DTO_DeactivateAccount DeactivateAccount { get; set; } = deactivateAccount;
}