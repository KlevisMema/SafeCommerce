/* 
 * Defines a MediatR command for updating a user.
 */

using MediatR;
using SafeCommerce.DataTransormObject.UserManagment;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Actions.Commands.UserManagment;

/// <summary>
/// Represents a MediatR command to update user information.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_UpdateUserCommand"/> class with the specified user ID and information.
/// </remarks>
/// <param name="id">The ID of the user to be updated.</param>
/// <param name="dTO_UserInfo">The user information to be updated.</param>
public class MediatR_UpdateUserCommand
(
    Guid id,
    DTO_UserInfo dTO_UserInfo
) : IRequest<Util_GenericResponse<DTO_UserUpdatedInfo>>
{
    /// <summary>
    /// Gets or sets the ID of the user to be updated.
    /// </summary>
    public Guid Id { get; set; } = id;
    /// <summary>
    /// Gets or sets the user information to be updated.
    /// </summary>
    public DTO_UserInfo DTO_UserInfo { get; set; } = dTO_UserInfo;
}