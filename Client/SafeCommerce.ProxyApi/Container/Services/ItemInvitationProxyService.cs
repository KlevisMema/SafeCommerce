using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ProxyApi.Container.Interfaces;

namespace SafeCommerce.ProxyApi.Container.Services;

public class ItemInvitationProxyService
(
    ILogger<ItemInvitationProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IItemInvitationProxyService
{
    public async Task<Util_GenericResponse<List<DTO_RecivedItemInvitation>>>
    GetItemsInvitations
    (
        string userId,
        string userIp,
        string jwtToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.GetItemsInvitations
                                                               .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_RecivedItemInvitation>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<List<DTO_RecivedItemInvitation>>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in GetItemsInvitations.");

            return new Util_GenericResponse<List<DTO_RecivedItemInvitation>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<Util_GenericResponse<List<DTO_SentItemInvitation>>>
    GetSentItemsInvitations
    (
        string userId,
        string userIp,
        string jwtToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());


            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.GetSentItemsInvitations
                                                              .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_SentItemInvitation>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<List<DTO_SentItemInvitation>>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in GetSentItemsInvitations.");

            return new Util_GenericResponse<List<DTO_SentItemInvitation>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    SendItemInvitation
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_SendItemInvitationRequest dTO_SendInvitation
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken),
                (nameof(fogeryToken), fogeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            var httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                fogeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            dTO_SendInvitation.InvitingUserId = Guid.Parse(userId);

            var content = new StringContent(JsonSerializer.Serialize(dTO_SendInvitation), Encoding.UTF8, "application/json");


            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.SendItemInvitation
                                                             .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, fogeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<bool>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in SendItemInvitation.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    AcceptItemInvitation
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_InvitationItemRequestActions acceptInvitationRequest
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken),
                (nameof(fogeryToken), fogeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            var httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                fogeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            acceptInvitationRequest.InvitedUserId = Guid.Parse(userId);

            var content = new StringContent(JsonSerializer.Serialize(acceptInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.AcceptItemInvitation
                                                             .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, fogeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<bool>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in AcceptItemInvitation.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = "Something went wrong",
                StatusCode = HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    RejectItemInvitation
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_InvitationItemRequestActions rejectInvitationRequest
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken),
                (nameof(fogeryToken), fogeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                fogeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            rejectInvitationRequest.InvitedUserId = Guid.Parse(userId);

            var content = new StringContent(JsonSerializer.Serialize(rejectInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.RejectItemInvitation
                                                             .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, fogeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<bool>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in RejectItemInvitation.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    DeleteItemInvitation
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_InvitationItemRequestActions deleteInvitationRequest
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp),
                (nameof(userId), userId),
                (nameof(jwtToken), jwtToken),
                (nameof(fogeryToken), fogeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                fogeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            deleteInvitationRequest.InvitingUserId = Guid.Parse(userId);

            var content = new StringContent(JsonSerializer.Serialize(deleteInvitationRequest), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.DeleteItemInvitation
                                                              .Replace("{userId}", userId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, fogeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<bool>()
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
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in DeleteItemInvitation.");

            return new Util_GenericResponse<bool>
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