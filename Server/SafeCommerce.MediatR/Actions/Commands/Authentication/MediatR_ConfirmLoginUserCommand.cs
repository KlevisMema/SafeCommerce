/* 
 * Defines a MediatR command for confirming a user's login using OTP (One-Time Password).
 * This command is processed by MediatR handlers to handle the login confirmation process.
 */

using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Authentication;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for confirming a user's login using a one-time password (OTP).
/// This command encapsulates the data required for the login confirmation process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_ConfirmLoginUserCommand"/> class.
/// </remarks>
/// <param name="oTP">The one-time password for login confirmation.</param>
/// <param name="userId">The unique identifier of the user attempting to log in.</param>    
public class MediatR_ConfirmLoginUserCommand
(
    string oTP,
    Guid userId
) : IRequest<Util_GenericResponse<DTO_LoginResult>>
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    public Guid UserId { get; set; } = userId;
    /// <summary>
    /// The OTP (One-Time Password) provided by the user.
    /// </summary>
    public string OTP { get; set; } = oTP;
}