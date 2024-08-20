namespace SafeCommerce.ProxyApi.Helpers;
public static class API_Helper_CookieHelper
{
    public static void StoreCookies(HttpResponseMessage response, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext != null && response.Headers.Contains("Set-Cookie"))
            httpContextAccessor.HttpContext.Items["Set-Cookie"] = response.Headers.GetValues("Set-Cookie").ToList();
    }
}