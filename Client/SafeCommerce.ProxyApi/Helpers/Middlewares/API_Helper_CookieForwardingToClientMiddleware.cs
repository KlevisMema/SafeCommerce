namespace SafeCommerce.ProxyApi.Helpers.Middlewares;

internal class API_Helper_CookieForwardingToClientMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Stream? originalBodyStream = context.Response.Body;

        using MemoryStream? responseBody = new();

        context.Response.Body = responseBody;

        await next(context);

        if (context.Items.ContainsKey("Set-Cookie"))
        {
            if (context.Items["Set-Cookie"] is List<string> cookies)
            {
                foreach (var cookie in cookies)
                    context.Response.Headers.Append("Set-Cookie", cookie);
            }
        }

        responseBody.Seek(0, SeekOrigin.Begin);

        await responseBody.CopyToAsync(originalBodyStream);
    }
}