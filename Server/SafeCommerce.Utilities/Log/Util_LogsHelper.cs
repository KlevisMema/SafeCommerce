/* 
 * Utility class for logging errors and returning a standard error response.
 * It captures the exception details and logs it along with the IP address.
*/

using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.IP;
using Microsoft.Extensions.Logging;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.Utilities.Log;

/// <summary>
/// Provides helper methods for logging errors and creating standardized error responses.
/// </summary>
public static class Util_LogsHelper<TReturnType, TLogger>
{
    /// <summary>
    /// Logs an internal server error with details from an exception and returns a standardized error response.
    /// </summary>
    /// <param name="ex">The exception that caused the error.</param>
    /// <param name="_logger">The logger instance to use for logging.</param>
    /// <param name="errorMessage">A custom error message to be logged.</param>
    /// <param name="value">Optional value to include in the response.</param>
    /// <param name="httpContextAccessor">Accessor to the HTTP context, used to fetch IP details.</param>
    /// <returns>A standardized error response with status code 500 (Internal Server Error).</returns>
    public static async Task<Util_GenericResponse<TReturnType>>
    ReturnInternalServerError
    (
        Exception ex,
        ILogger<TLogger> _logger,
        string errorMessage,
        TReturnType? value,
        IHttpContextAccessor httpContextAccessor

    )
    {
        var errorResponse = Util_GenericResponse<TReturnType>.Response
        (
            value,
            false,
            ex.ToString(),
            null,
            System.Net.HttpStatusCode.InternalServerError
        );

        _logger.LogCritical
        (
            ex,
            "{errorMessage}, [IP] {IP}",
            errorResponse,
            await Util_GetIpAddres.GetLocation(httpContextAccessor)
        );

        errorResponse.Message = "Internal server error";

        return errorResponse;
    }
}