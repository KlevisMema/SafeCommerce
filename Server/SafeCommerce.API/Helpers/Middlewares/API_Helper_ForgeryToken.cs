using Microsoft.AspNetCore.Antiforgery;

namespace SafeCommerce.API.Helpers.Middlewares;

/// <summary>
/// A middleware that assigns a token to the response.
/// </summary>
public class API_Helper_ForgeryToken(IAntiforgery antiforgery, RequestDelegate next)
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity!.IsAuthenticated)
        {
            string key = "XSRF-TOKEN-" + context.User.Identity.Name;
            string key2 = "Cookie-XSRF-TOKEN-" + context.User.Identity.Name;

            if (String.IsNullOrEmpty(context.Request.Cookies["X-XSRF-TOKEN"]))
            {
                var token = antiforgery.GetTokens(context);

                context.Response.Cookies.Append(key, token.RequestToken!, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                });

                context.Response.Cookies.Append(key2, token.CookieToken!, new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict,
                });
            }
        }

        await next(context);
    }
}