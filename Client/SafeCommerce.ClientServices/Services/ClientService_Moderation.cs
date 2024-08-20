using System.Text.Json;
using SafeCommerce.ClientDTO.Moderation;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.ClientServices.Services;
public class ClientService_Moderation
(
    IHttpClientFactory httpClientFactory
) : IClientService_Moderation
{
    public async Task<ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>>
    GetModerationHistoryForModerator()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.ProxyRouteModeration + Route_Moderation.ProxyGetModerationHistoryForModerator);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>>
            (
                responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
}