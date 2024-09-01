using System.Text;
using System.Text.Json;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.ClientServices.Services;

public class ClientService_CheckOut
(
    IHttpClientFactory httpClientFactory
) : IClientService_CheckOut
{
    public async Task<string>
    CheckOut
    (
        string userId,
        List<ClientDto_CartItem> CartItems
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var content = new StringContent(JsonSerializer.Serialize(CartItems), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.ProxyRouteCheckOut}{Route_CheckOut.ProxyCheckOut.Replace("{userId}", userId)}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<string>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (Exception)
        {
            throw;
        }
    }
}