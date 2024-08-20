/* 
 * Defines a MediatR command handler for processing refresh token requests.
 * This handler is responsible for invoking the appropriate authentication service to handle the refresh token process, allowing users to obtain a new JWT token without re-authentication.
 */

using MediatR;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.MediatR.Dependencies;
using SafeCommerce.Authentication.Interfaces;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.MediatR.Actions.Commands.Authentication;

namespace SafeCommerce.MediatR.Handlers.CommandsHandlers.Authentication;

/// <summary>
/// A MediatR command handler for processing refresh token requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MediatR_RefreshTokenCommandHandler"/> class.
/// </remarks>
/// <param name="service">The authentication service used for refreshing JWT tokens.</param>
public class MediatR_RefreshTokenCommandHandler
(
    IAUTH_RefreshToken service
) : MediatR_GenericHandler<IAUTH_RefreshToken>(service),
    IRequestHandler<MediatR_RefreshTokenCommand, Util_GenericResponse<DTO_Token>>
{
    /// <summary>
    /// Handles the refresh token process.
    /// </summary>
    /// <param name="request">The command containing the data for the refresh token request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An ObjectResult with the outcome of the refresh token process.</returns>
    public async Task<Util_GenericResponse<DTO_Token>>
    Handle
    (
        MediatR_RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var refreshTokenResult = await _service.RefreshToken(request.DTO_ValidateToken);

        return refreshTokenResult;
    }
}