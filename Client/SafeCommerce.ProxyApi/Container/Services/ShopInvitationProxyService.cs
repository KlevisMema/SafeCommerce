using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.ProxyApi.Container.Interfaces;

namespace SafeCommerce.ProxyApi.Container.Services;

public class ShopInvitationProxyService
(
    ILogger<ShopInvitationProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IShopInvitationProxyService
{
    public async Task<Util_GenericResponse<List<DTO_RecivedInvitations>>> 
    GetShopsInvitations
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

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.GetShopsInvitations
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

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_RecivedInvitations>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<List<DTO_RecivedInvitations>>()
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
            logger.LogCritical(ex, "Exception in GetShopsInvitations.");

            return new Util_GenericResponse<List<DTO_RecivedInvitations>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.ArgumentNullException,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<Util_GenericResponse<List<DTO_SentInvitations>>> 
    GetSentShopInvitations
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


            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.GetSentShopInvitations
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

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_SentInvitations>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                return new Util_GenericResponse<List<DTO_SentInvitations>>()
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
            logger.LogCritical(ex, "Exception in GetSentShopInvitations.");

            return new Util_GenericResponse<List<DTO_SentInvitations>>
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
    SendInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_SendInvitationRequest dTO_SendInvitation
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


            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.SendInvitation
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
            logger.LogCritical(ex, "Exception in SendInvitation.");

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
    AcceptInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions acceptInvitationRequest
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

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.AcceptInvitation
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
            logger.LogCritical(ex, "Exception in AcceptInvitation.");

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
    RejectInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions rejectInvitationRequest
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

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.RejectInvitation
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
            logger.LogCritical(ex, "Exception in RejectInvitation.");

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
    DeleteInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions deleteInvitationRequest
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

            string? requestUrl = $"{BaseRoute.RouteInvitationForClient}{Route_InvitationRoute.DeleteInvitation
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
            logger.LogCritical(ex, "Exception in DeleteInvitation.");

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