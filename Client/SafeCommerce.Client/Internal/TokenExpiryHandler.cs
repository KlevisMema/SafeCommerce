using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace SafeCommerce.Client.Internal;

internal class TokenExpiryHandler
(
    IClientAuthentication_TokenRefreshService tokenRefreshService,
    NavigationManager navigationManager,
    ILocalStorageService localStorage
) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage>
    SendAsync
    (
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenRefreshed = await tokenRefreshService.RefreshToken();

                if (tokenRefreshed)
                    return await base.SendAsync(request, cancellationToken);
                else
                {
                    await localStorage.SetItemAsync("SessionExpired", true, cancellationToken);
                    navigationManager.NavigateTo("/");
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                await localStorage.SetItemAsync("SessionExpired", true, cancellationToken);
                navigationManager.NavigateTo("/");
            }

            return response;

        }
        catch (Exception)
        {
            await localStorage.SetItemAsync("SessionExpired", true, cancellationToken);
            navigationManager.NavigateTo("/Login");
        }

        return new HttpResponseMessage();
    }
}