/*
 * Utility class dedicated to retrieving the IP address from a given HTTP context.
 * The utility can identify local IP addresses and attempt to fetch the external IP when necessary.
*/

using Microsoft.AspNetCore.Http;

namespace SafeCommerce.Utilities.IP;

/// <summary>
/// Provides utility methods related to IP address retrieval and location determination.
/// </summary>
public static class Util_GetIpAddres
{
    /// <summary>
    /// Retrieves the location based on the IP address from the given HTTP context accessor.
    /// If the IP address is local (":::1"), it tries to fetch the external IP using an external service.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <returns>A string representation of the IP address or an empty string in case of failure.</returns>
    public static async Task<string> GetLocation(IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            string? result = httpContextAccessor.HttpContext.Request.Headers["X-Original-Client-IP"];

            if (string.IsNullOrEmpty(result))
                result = httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (result == "::1")
            {
                HttpResponseMessage response;

                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.ipify.org/");
                var client = new HttpClient();
                response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    StreamReader reader = new StreamReader(responseStream);
                    result = await reader.ReadToEndAsync();
                }
            }

            return result ?? string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}