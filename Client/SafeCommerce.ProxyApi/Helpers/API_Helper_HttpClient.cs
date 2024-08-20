using System.Net;
using System.Net.Security;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace SafeCommerce.ProxyApi.Helpers;

internal static class API_Helper_HttpClient
{
    /// <summary>
    ///    Use to pass specific tokens in a cookie for 
    ///    antiforgery purpose. Use only for post, edit or delete. 
    /// </summary>
    /// <param name="baseMainApiAddr"> The base addr or the safeshare api </param>
    /// <param name="aspNetForgeryToken"> The asp net default token </param>
    /// <param name="fogeryToken"> The custom token </param>
    /// <returns> A new <see cref="HttpClient"/> with cookies attached</returns>
    public static HttpClient
    NewClientWithCookies
    (
        string baseMainApiAddr,
        string aspNetForgeryToken,
        string fogeryToken,
        string cookieXsrfTokenName,
        string xsrfTokenName
    )
    {
        var httpClientHandler = ClientHandler();

        var cookieContainer = new CookieContainer();

        cookieContainer.Add(new Uri(baseMainApiAddr), new Cookie(cookieXsrfTokenName, aspNetForgeryToken));
        cookieContainer.Add(new Uri(baseMainApiAddr), new Cookie(xsrfTokenName, fogeryToken));

        httpClientHandler.CookieContainer = cookieContainer;

        return new HttpClient(httpClientHandler) { BaseAddress = new Uri(baseMainApiAddr) };
    }
    /// <summary>
    /// Adds custom headers and common headers (JWT token) to the specified HttpRequestMessage.
    /// </summary>
    /// <param name="jwtToken">The JWT token to be included in the Authorization header.</param>
    /// <param name="httpRequestMessage">The HttpRequestMessage to which headers will be added.</param>
    /// <param name="headers">Additional headers to be added to the request.</param>
    public static void
    AddHeadersToTheRequest
    (
        string jwtToken,
        HttpRequestMessage httpRequestMessage,
        params KeyValuePair<string, string>[] headers
    )
    {
        foreach (var header in headers)
            httpRequestMessage.Headers.Add(header.Key, header.Value);

        if (!String.IsNullOrEmpty(jwtToken))
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
    }
    /// <summary>
    ///     Creates a client based on a name and client factory.
    /// </summary>
    /// <param name="clientName">The name of the client</param>
    /// <param name="httpClientFactory">The <see cref="IHttpClientFactory"/> reference</param>
    /// <returns></returns>
    public static HttpClient
    CreateClientInstance
    (
        string baseMainApiAddr
    )
    {
        var httpClientHandler = ClientHandler();

        return new HttpClient(httpClientHandler) { BaseAddress = new Uri(baseMainApiAddr) };
    }
    /// <summary>
    /// Create a custom client handler with custom certeficate check.
    /// </summary>
    /// <returns> The client handle </returns>
    private static HttpClientHandler 
    ClientHandler()
    {
        var httpClientHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) =>
            {
                if (sslPolicyErrors == SslPolicyErrors.None)
                {
                    var publicKey = certificate.GetPublicKey();

                    var publicKey2 = certificate.GetPublicKeyString();

                    using var sha256 = SHA256.Create();

                    var publicKeyHashBytes = sha256.ComputeHash(publicKey);

                    var publicKeyHash = BitConverter.ToString(publicKeyHashBytes).Replace("-", "").ToLower();

                    return publicKeyHash == Environment.GetEnvironmentVariable("MAIN_API_PUBLIC_KEY_HASH");
                }

                return false;
            }
        };

        return httpClientHandler;
    }
}