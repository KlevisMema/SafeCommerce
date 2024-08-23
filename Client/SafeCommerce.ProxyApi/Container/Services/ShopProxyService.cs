#region Usings
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Shop;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using System.Net.NetworkInformation;

#endregion

namespace SafeCommerce.ProxyApi.Container.Services;

public class ShopProxyService
    (
    ILogger<ShopProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IShopProxyService
{
    #region Get
    public async Task<Util_GenericResponse<DTO_Shop>>
    GetShop
    (
        Guid ownerId,
        Guid shopId,
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
                (nameof(shopId), shopId.ToString()),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.GetShop
                                                                .Replace("{shopId}", shopId.ToString())
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

            Util_GenericResponse<DTO_Shop> readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Shop>>
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
                return new Util_GenericResponse<DTO_Shop>()
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
            logger.LogCritical(ex, "Exception in GetShop.");

            return new Util_GenericResponse<DTO_Shop>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<List<DTO_Shop>>>
    GetUserShops
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

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.GetShops
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

            Util_GenericResponse<List<DTO_Shop>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_Shop>>>
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
                return new Util_GenericResponse<List<DTO_Shop>>()
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
            logger.LogCritical(ex, "Exception in GetUserShops.");

            return new Util_GenericResponse<List<DTO_Shop>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_ShopMembers>>>
    GetMembersOfTheShop
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
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi()
            );

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.GetMembersOfTheShop
                                                            .Replace("{shopId}", shopId.ToString())
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

            Util_GenericResponse<IEnumerable<DTO_ShopMembers>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<IEnumerable<DTO_ShopMembers>>>
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
                return new Util_GenericResponse<IEnumerable<DTO_ShopMembers>>()
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
            logger.LogCritical(ex, "Exception in GetMembersOfTheShop.");

            return new Util_GenericResponse<IEnumerable<DTO_ShopMembers>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Shop>>>
    GetPublicSharedShops
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

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.GetPublicShops
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

            Util_GenericResponse<IEnumerable<DTO_Shop>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<IEnumerable<DTO_Shop>>>
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
                return new Util_GenericResponse<IEnumerable<DTO_Shop>>()
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
            logger.LogCritical(ex, "Exception in GetPublicSharedShops.");

            return new Util_GenericResponse<IEnumerable<DTO_Shop>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>
    GetShopsSubjectForModeration
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

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.GetShopsForModeration
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

            Util_GenericResponse<IEnumerable<DTO_ShopForModeration>> readResult = JsonSerializer.Deserialize<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>
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
                return new Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>()
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
            logger.LogCritical(ex, "Exception in GetUserShops.");

            return new Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>
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
    public async Task<Util_GenericResponse<DTO_Shop>>
    CreateShop
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_CreateShop createShopDto
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

            Dictionary<string, string>? registerData = new()
                {
                    { nameof(DTO_CreateShop.Name), createShopDto.Name },
                    { nameof(DTO_CreateShop.Description), createShopDto.Description },
                    { nameof(DTO_CreateShop.EncryptedSymmetricKey), createShopDto.EncryptedSymmetricKey },
                    { nameof(DTO_CreateShop.DataNonce), createShopDto.DataNonce },
                    { nameof(DTO_CreateShop.EncryptedKeyNonce), createShopDto.EncryptedKeyNonce },
                    { nameof(DTO_CreateShop.SigningPublicKey), createShopDto.SigningPublicKey },
                    { nameof(DTO_CreateShop.SignatureOfKey), createShopDto.SignatureOfKey },
                    { nameof(DTO_CreateShop.MakePublic), createShopDto.MakePublic.ToString() },
                };

            FormUrlEncodedContent? contentForm = new(registerData);

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.CreateShop.Replace("{userId}", ownerId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = contentForm
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

            Util_GenericResponse<DTO_Shop>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Shop>>
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
                return new Util_GenericResponse<DTO_Shop>()
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
            logger.LogCritical(ex, "Exception in CreateShop.");

            return new Util_GenericResponse<DTO_Shop>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    InviteUserToShop
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_InviteUserToShop inviteUserToShopDto
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
                (nameof(shopId), shopId.ToString()),
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

            StringContent? content = new(JsonSerializer.Serialize(inviteUserToShopDto), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.InviteUserToShop
                                                                    .Replace("{shopId}", shopId.ToString())
                                                                    .Replace("{userId}", ownerId.ToString())}";

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
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in InviteUserToShop.");

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
    MooderateShop
    (
        string userIp,
        string jwtToken,
        Guid moderatorId,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_ModerateShop moderateShop
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(moderatorId), moderatorId.ToString()),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken),
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

            StringContent? content = new(JsonSerializer.Serialize(moderateShop), Encoding.UTF8, "application/json");

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.ModerateShop
                                                                    .Replace("{userId}", moderatorId.ToString())}";

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
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in MooderateShop.");

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
    public async Task<Util_GenericResponse<DTO_Shop>>
    EditShop
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_UpdateShop editShopDto
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
                (nameof(shopId), shopId.ToString()),
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

            Dictionary<string, string>? registerData = new()
            {
                { nameof(DTO_UpdateShop.Name), editShopDto.Name },
                { nameof(DTO_UpdateShop.Description), editShopDto.Description },
                { nameof(DTO_UpdateShop.EncryptedKeyNonce), editShopDto.EncryptedKeyNonce },
                { nameof(DTO_UpdateShop.DataNonce), editShopDto.DataNonce },
                { nameof(DTO_UpdateShop.EncryptedSymmetricKey), editShopDto.EncryptedSymmetricKey },
                { nameof(DTO_UpdateShop.SigningPublicKey), editShopDto.SigningPublicKey },
                { nameof(DTO_UpdateShop.SignatureOfKey), editShopDto.SignatureOfKey },
            };

            FormUrlEncodedContent? contentForm = new(registerData);

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.EditShop
                                                                .Replace("{shopId}", shopId.ToString())
                                                                .Replace("{userId}", ownerId.ToString())}";

            HttpRequestMessage? requestMessage = new(HttpMethod.Put, requestUrl)
            {
                Content = contentForm
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

            Util_GenericResponse<DTO_Shop>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Shop>>
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
                return new Util_GenericResponse<DTO_Shop>()
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
            logger.LogCritical(ex, "Exception in EditShop.");

            return new Util_GenericResponse<DTO_Shop>
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
    DeleteShop
    (
        Guid shopId,
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
                (nameof(shopId), shopId.ToString()),
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

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.DeleteShop
                                                                 .Replace("{shopId}", shopId.ToString())
                                                                 .Replace("{userId}", ownerId.ToString())}";

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
            logger.LogCritical(ex, "Exception in DeleteShop.");

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
    RemoveUserFromShop
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_RemoveUserFromShop dTO_RemoveUserFromShop
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

            HttpClient? httpClient = API_Helper_HttpClient.NewClientWithCookies
            (
                requestConfigurationProxyService.GetBaseAddrOfMainApi(),
                aspNetForgeryToken,
                forgeryToken,
                requestHeaderOptions.Value.AspNetCoreAntiforgery,
                requestHeaderOptions.Value.XSRF_TOKEN
            );

            string? requestUrl = $"{BaseRoute.RouteShopForClient}{Route_ShopRoutes.RemoveUserFromShop
                                                                 .Replace("{userId}", ownerId.ToString())}";

            string? json = JsonSerializer.Serialize(dTO_RemoveUserFromShop);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, requestUrl)
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
                    Value = false,
                };
            }
            else
                throw;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in RemoveUserFromShop.");

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