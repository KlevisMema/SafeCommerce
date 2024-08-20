/* 
 * Defines a MediatR command for handling the token refresh process.
 * This command is intended for use within MediatR handlers to facilitate the refreshing of JWT tokens, ensuring continuous and secure authentication.
 */

using MediatR;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.MediatR.Actions.Commands.Authentication;

/// <summary>
/// Represents a MediatR command for refreshing a JWT (JSON Web Token).
/// This command includes the necessary data for the token refresh process.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_RefreshTokenCommand"/> class.
/// </remarks>
/// <param name="dTO_ValidateToken">The DTO containing token validation and refresh data.</param>
public class MediatR_RefreshTokenCommand
(
    DTO_ValidateToken dTO_ValidateToken
) : IRequest<Util_GenericResponse<DTO_Token>>
{
    /// <summary>
    /// Data transfer object containing the information required for token validation and refresh.
    /// </summary>
    public DTO_ValidateToken DTO_ValidateToken { get; set; } = dTO_ValidateToken;
}