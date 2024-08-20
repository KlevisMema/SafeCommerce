using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.ClientServices.Authentication;

public class ClientAuthentication_TokenRefreshService(IHttpClientFactory httpClientFactory) : IClientAuthentication_TokenRefreshService
{
    public async Task<bool>
    RefreshToken()
    {
        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.RefreshToken, null);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}