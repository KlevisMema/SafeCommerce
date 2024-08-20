/* 
 * Provides utility functions for user identity operations within the application. 
 * This class includes methods to retrieve user information from the security token.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace SafeCommerce.Utilities.User;

/// <summary>
/// Contains utility methods for extracting user information from security tokens.
/// </summary>
public static class Util_FindUserIdFromToken
{
    /// <summary>
    /// Retrieves the user ID from the current HTTP context's security token.
    /// </summary>
    /// <param name="httpContextAccessor">The accessor used to obtain the HTTP context.</param>
    /// <returns>The user ID if found; otherwise, an empty string.</returns>
    public static string
    UserId
    (
        IHttpContextAccessor httpContextAccessor
    )
    {
        string? userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return "";

        return userId;
    }
}