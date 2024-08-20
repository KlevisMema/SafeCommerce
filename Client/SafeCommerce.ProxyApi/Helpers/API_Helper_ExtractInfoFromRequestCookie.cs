using SafeCommerce.Security.JwtSecurity.Helpers;

namespace SafeCommerce.ProxyApi.Helpers;

internal static class API_Helper_ExtractInfoFromRequestCookie
{
    public static string
    GetUserIp
    (
        string headerName,
        HttpRequest Request
    )
    {
        return Request.Headers[headerName].ToString();
    }

    public static string
    UserId
    (
        string jwtToken
    )
    {
        return Helper_JwtToken.GetUserIdDirectlyFromJwtToken(jwtToken) ?? string.Empty;
    }

    public static string
    JwtToken
    (
        string cookieName,
        HttpRequest Request
    )
    {
        return Request.Cookies[cookieName] ?? string.Empty;
    }

    public static string
    GetForgeryToken
    (
        string cookieName,
        HttpRequest Request
    )
    {
        try
        {
            string? key = $"{cookieName}-{Request?.HttpContext?.User?.Identity?.Name}";
            return Request?.Cookies[key]?.ToString() ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public static string
    GetAspNetCoreForgeryToken
    (
        string cookieName,
        HttpRequest Request
    )
    {
        try
        {
            string? key = $"{cookieName}-{Request?.HttpContext?.User?.Identity?.Name}";
            return Request?.Cookies[key]?.ToString() ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}