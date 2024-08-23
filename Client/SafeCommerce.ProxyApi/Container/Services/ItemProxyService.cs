#region Usings
using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.Moderation; 
#endregion

namespace SafeCommerce.ProxyApi.Container.Services;

public class ItemProxyService
(
    ILogger<ItemProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IItemProxyService
{
    #region Get
    public async Task<Util_GenericResponse<DTO_Item>>
    GetItemDetails
    (
        Guid itemId,
        Guid ownerId,
        string userIp,
        string jwtToken
    )
        {
            HttpResponseMessage response = new();

            try
            {
                API_Helper_ParamsStringChecking.CheckNullOrEmpty
                (
                    (nameof(ownerId), ownerId.ToString()),
                    (nameof(userIp), userIp),
                    (nameof(jwtToken), jwtToken),
                    (nameof(itemId), itemId.ToString())
                );

                HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
                (
                    requestConfigurationProxyService.GetBaseAddrOfMainApi()
                );

                string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.GetItemDetails
                                                                    .Replace("{userId}", ownerId.ToString())
                                                                    .Replace("{itemId}", itemId.ToString())}";

                HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

                API_Helper_HttpClient.AddHeadersToTheRequest
                (
                    jwtToken,
                    requestMessage,
                    new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
                );

                response = await httpClient.SendAsync(requestMessage);

                string? responseContent = await response.Content.ReadAsStringAsync();

                Util_GenericResponse<DTO_Item>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Item>>
                (
                    responseContent,
                    new JsonSerializerOptions
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
                    return new Util_GenericResponse<DTO_Item>()
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
                logger.LogCritical(ex, "Exception in GetItemDetails.");

                return new Util_GenericResponse<DTO_Item>
                {
                    Errors = null,
                    Message = ClientUtil_ExceptionResponse.GeneralMessage,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null,
                };
            }
        }

    public async Task<Util_GenericResponse<List<DTO_Item>>>
    GetItemsByShopId
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken),
                (nameof(shopId), shopId.ToString())
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.GetItemsByShopId
                                                            .Replace("{userId}", ownerId.ToString())
                                                            .Replace("{shopId}", shopId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<List<DTO_Item>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_Item>>>
            (
                responseContent,
                new JsonSerializerOptions
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
                return new Util_GenericResponse<List<DTO_Item>>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in GetItemsByShopId.");

            return new Util_GenericResponse<List<DTO_Item>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>>
    GetUserItems
    (
        Guid ownerId,
        string userIp,
        string jwtToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.GetUserItems
                                                                .Replace("{userId}", ownerId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<IEnumerable<DTO_Item>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<IEnumerable<DTO_Item>>>
            (
                responseContent,
                new JsonSerializerOptions
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
                return new Util_GenericResponse<IEnumerable<DTO_Item>>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in GetUserItems.");

            return new Util_GenericResponse<IEnumerable<DTO_Item>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>>
    GetItemsSubjectForModeration
    (
        Guid moderatorId,
        string userIp,
        string jwtToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(moderatorId), moderatorId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.GetItemsForModeration
                                                                .Replace("{userId}", moderatorId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Get, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<IEnumerable<DTO_Item>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<IEnumerable<DTO_Item>>>
            (
                responseContent,
                new JsonSerializerOptions
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
                return new Util_GenericResponse<IEnumerable<DTO_Item>>()
                {
                    Message = argException.Message,
                    Errors = null,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in GetItemsSubjectForModeration.");

            return new Util_GenericResponse<IEnumerable<DTO_Item>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }
    #endregion

    #region Post
    public async Task<Util_GenericResponse<bool>>
    CreateItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_CreateItem createItemDto
    )
        {
            HttpResponseMessage response = new();

            try
            {
                API_Helper_ParamsStringChecking.CheckNullOrEmpty
                (
                    (nameof(ownerId), ownerId.ToString()),
                    (nameof(userIp), userIp),
                    (nameof(jwtToken), jwtToken),
                    (nameof(forgeryToken), forgeryToken),
                    (nameof(aspNetForgeryToken), aspNetForgeryToken)
                );

                HttpClient httpClient = API_Helper_HttpClient.NewClientWithCookies
                (
                    requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                    aspNetForgeryToken,
                    forgeryToken,
                    requestHeaderOptions.Value.AspNetCoreAntiforgery,
                    requestHeaderOptions.Value.XSRF_TOKEN
                );

                StringContent content = new(JsonSerializer.Serialize(createItemDto), Encoding.UTF8, "application/json");

                string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.CreateItem.Replace("{userId}", ownerId.ToString())}";

                HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
                {
                    Content = content
                };

                API_Helper_HttpClient.AddHeadersToTheRequest
                (
                    jwtToken,
                    requestMessage,
                    new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, forgeryToken),
                    new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                    new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
                );

                response = await httpClient.SendAsync(requestMessage);

                string? responseContent = await response.Content.ReadAsStringAsync();

                Util_GenericResponse<bool>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>
                (
                    responseContent,
                    new JsonSerializerOptions
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
                    return new Util_GenericResponse<bool>()
                    {
                        Message = argException.Message,
                        Errors = null,
                        StatusCode = response.StatusCode,
                        Succsess = false,
                        Value = false
                    };
                }
                else
                    throw;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Exception in CreateItem.");

                return new Util_GenericResponse<bool>
                {
                    Errors = null,
                    Message = ClientUtil_ExceptionResponse.GeneralMessage,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false,
                };
            }
        }

    public async Task<Util_GenericResponse<bool>>
    ShareItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        DTO_ShareItem shareItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            StringContent content = new(JsonSerializer.Serialize(shareItemDto), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.ShareItem.Replace("{userId}", ownerId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<bool>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>
            (
                responseContent,
                new JsonSerializerOptions
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
            logger.LogCritical(ex, "Exception in ShareItem.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false,
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ModerateItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        DTO_ModerateItem moderateItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            StringContent content = new(JsonSerializer.Serialize(moderateItemDto), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.ModerateItem.Replace("{userId}", ownerId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<bool>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>
            (
                responseContent,
                new JsonSerializerOptions
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
            logger.LogCritical(ex, "Exception in ModerateItem.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false,
            };
        }
    }
    #endregion

    #region Put
    public async Task<Util_GenericResponse<DTO_Item>>
    EditItem
    (
        Guid itemId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_UpdateItem editItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken),
                (nameof(itemId), itemId.ToString()),
                (nameof(forgeryToken), forgeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                forgeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            StringContent? content = new(JsonSerializer.Serialize(editItemDto), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.EditItem
                                                                .Replace("{userId}", ownerId.ToString())
                                                                .Replace("{itemId}", itemId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Put, requestUrl)
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, forgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<DTO_Item>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Item>>
            (
                responseContent,
                new JsonSerializerOptions
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
                return new Util_GenericResponse<DTO_Item>()
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
            logger.LogCritical(ex, "Exception in EditItem.");

            return new Util_GenericResponse<DTO_Item>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }
    #endregion

    #region Delete
    public async Task<Util_GenericResponse<bool>>
    DeleteItem
    (
       Guid itemId,
       Guid ownerId,
       string userIp,
       string jwtToken,
       string forgeryToken,
       string aspNetForgeryToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(ownerId), ownerId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken),
                (nameof(itemId), itemId.ToString()),
                (nameof(forgeryToken), forgeryToken),
                (nameof(aspNetForgeryToken), aspNetForgeryToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                forgeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            string? requestUrl = $"{BaseRoute.RouteItemForClient}{Route_ItemRoutes.DeleteItem
                                                                .Replace("{userId}", ownerId.ToString())
                                                                .Replace("{itemId}", itemId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, requestUrl);

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.XSRF_TOKEN, forgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.AspNetCoreAntiforgery, aspNetForgeryToken),
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<bool>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<bool>>
            (
                responseContent,
                new JsonSerializerOptions
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
            logger.LogCritical(ex, "Exception in DeleteItem.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false,
            };
        }
    } 
    #endregion
}