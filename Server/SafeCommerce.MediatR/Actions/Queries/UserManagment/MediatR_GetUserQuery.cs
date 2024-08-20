/* 
 * Defines a MediatR query for retrieving a user based on their ID.
*/

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SafeCommerce.MediatR.Actions.Queries.UserManagment;

/// <summary>
/// Represents a MediatR query to fetch a user by their ID.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_GetUserQuery"/> class with the specified user ID.
/// </remarks>
/// <param name="id">The ID of the user to be retrieved.</param>
public class MediatR_GetUserQuery
(
    Guid id
) : IRequest<ObjectResult>
{
    /// <summary>
    /// Gets or sets the ID of the user to be retrieved.
    /// </summary>
    public Guid Id { get; set; } = id;
}