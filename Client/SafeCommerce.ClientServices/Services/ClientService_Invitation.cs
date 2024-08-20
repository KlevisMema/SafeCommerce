using System.Net;
using System.Text;
using System.Text.Json;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Services;

public class ClientService_Invitation
(
    IHttpClientFactory httpClientFactory
) : IClientService_Invitation
{
    public async Task<ClientUtil_ApiResponse<List<ClientDto_RecivedInvitations>>> GetShopsInvitations()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxyGetShopsInvitations}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<List<ClientDto_RecivedInvitations>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<List<ClientDto_RecivedInvitations>>()
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
            return new ClientUtil_ApiResponse<List<ClientDto_RecivedInvitations>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<List<ClientDto_SentInvitations>>> GetSentShopInvitations()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxyGetSentShopInvitations}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<List<ClientDto_SentInvitations>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<List<ClientDto_SentInvitations>>()
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
            return new ClientUtil_ApiResponse<List<ClientDto_SentInvitations>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>> SendInvitation(ClientDto_SendInvitationRequest dTO_SendInvitation)
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var content = new StringContent(JsonSerializer.Serialize(dTO_SendInvitation), Encoding.UTF8, "application/json");


            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxySendInvitation}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<bool>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>> AcceptInvitation(ClientDto_InvitationRequestActions acceptInvitationRequest)
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var content = new StringContent(JsonSerializer.Serialize(acceptInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxyAcceptInvitation}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<bool>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = "Something went wrong",
                StatusCode = HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>> RejectInvitation(ClientDto_InvitationRequestActions rejectInvitationRequest)
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var content = new StringContent(JsonSerializer.Serialize(rejectInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxyRejectInvitation}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<bool>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>> DeleteInvitation(ClientDto_InvitationRequestActions deleteInvitationRequest)
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var content = new StringContent(JsonSerializer.Serialize(deleteInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteProxyShopInvitationForClient}{Route_InvitationRoute.ProxyDeleteInvitation}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, requestUrl)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new ClientUtil_ApiResponse<bool>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = false
            };
        }
    }
}