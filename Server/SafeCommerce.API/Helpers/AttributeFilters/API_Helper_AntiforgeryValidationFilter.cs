using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SafeCommerce.API.Helpers.AttributeFilters;

/// <summary>
/// Helper for anti forgery token
/// </summary>
/// <param name="antiforgery"></param>
public class API_Helper_AntiforgeryValidationFilter(IAntiforgery antiforgery) : IAsyncAuthorizationFilter
{
    /// <summary>
    /// Authorizes the request and checks for the token
    /// </summary>
    /// <param name="context">The filter authorization context</param>
    /// <returns>Authorization result</returns>
    public async Task
    OnAuthorizationAsync
    (
        AuthorizationFilterContext context
    )
    {
        await antiforgery.ValidateRequestAsync(context.HttpContext);
    }
}