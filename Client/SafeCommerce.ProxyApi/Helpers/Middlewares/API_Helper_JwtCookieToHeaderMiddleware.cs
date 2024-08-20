using Microsoft.Extensions.Options;

namespace SafeCommerce.ProxyApi.Helpers.Middlewares;

internal class API_Helper_JwtCookieToHeaderMiddleware
(
    RequestDelegate next,
    IOptions<API_Helper_RequestHeaderSettings> options
)
{
    public async Task
    Invoke
    (
        HttpContext context
    )
    {
        if (context.Request.Cookies.TryGetValue("authToken", out var token))
            context.Request.Headers.Append("Authorization", "Bearer " + token);

        await next(context);
    }
}