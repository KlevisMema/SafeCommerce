using System.Text;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Connections.Features;

namespace SafeCommerce.ProxyApi.Helpers.Middlewares;

public class API_HELPER_MetadataLoggingMiddleware(RequestDelegate next, ILogger<API_HELPER_MetadataLoggingMiddleware> logger)
{
    private readonly ILogger<API_HELPER_MetadataLoggingMiddleware> _logger = logger;

    public async Task
    InvokeAsync
    (
        HttpContext context
    )
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path;
        var queryString = context.Request.QueryString.Value;
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var referer = context.Request.Headers["Referer"].ToString();
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        var tlsVersion = context.Features.Get<ITlsHandshakeFeature>()?.Protocol;
        var requestBodySize = context.Request.ContentLength ?? 0;

        string requestBody = string.Empty;
        if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
        {
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            context.Request.Body.Position = 0;
        }

        var userName = context.User.Identity?.Name ?? "Anonymous";
        var userRoles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var cookies = string.Join("; ", context.Request.Cookies.Select(c => $"{c.Key}={c.Value}"));

        await next(context);

        var statusCode = context.Response.StatusCode;
        var responseSize = context.Response.ContentLength ?? 0;

        stopwatch.Stop();
        var executionTime = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation("Request Details: Method={Method}, Path={Path}, QueryString={QueryString}, UserAgent={UserAgent}, Referer={Referer}, ClientIp={ClientIp}, TlsVersion={TlsVersion}, " +
            "RequestBodySize={RequestBodySize}, UserName={UserName}, UserRoles={UserRoles}, Cookies={Cookies}, " +
            "ResponseStatusCode={StatusCode}, ResponseSize={ResponseSize}, ExecutionTime={ExecutionTime}ms",
            method, path, queryString, userAgent, referer, clientIp, tlsVersion,
            requestBodySize, userName, string.Join(",", userRoles), cookies,
            statusCode, responseSize, executionTime);

        if (!string.IsNullOrEmpty(requestBody))
        {
            _logger.LogDebug("Request Body: {RequestBody}", requestBody);
        }
    }
}