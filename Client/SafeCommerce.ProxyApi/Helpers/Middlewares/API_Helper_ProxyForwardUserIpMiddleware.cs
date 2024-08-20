namespace SafeCommerce.ProxyApi.Helpers.Middlewares;

internal class API_Helper_ProxyForwardUserIpMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        string clientIpAddress = context.Connection.RemoteIpAddress.ToString();

        context.Request.Headers.Add("X-Original-Client-IP", clientIpAddress);

        await next(context);
    }
}