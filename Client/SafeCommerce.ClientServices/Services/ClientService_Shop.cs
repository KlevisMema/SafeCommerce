#region Usings
using System.Text;
using System.Text.Json;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientServices.Interfaces; 
#endregion

namespace SafeCommerce.ClientServices.Services;

public class ClientService_Shop
(
    IHttpClientFactory httpClientFactory
) : IClientService_Shop
{
    #region Get
    public async Task<ClientUtil_ApiResponse<ClientDto_Shop>>
    GetShop
    (
        Guid shopId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyGetShop.Replace("{shopId}", shopId.ToString()));

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Shop>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Shop>>
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
                return new ClientUtil_ApiResponse<ClientDto_Shop>()
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
            return new ClientUtil_ApiResponse<ClientDto_Shop>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<List<ClientDto_Shop>>>
    GetUserShops()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyGetShops);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<List<ClientDto_Shop>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<List<ClientDto_Shop>>>
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
                return new ClientUtil_ApiResponse<List<ClientDto_Shop>>()
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
            return new ClientUtil_ApiResponse<List<ClientDto_Shop>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>>
    GetPublicSharedShops()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyGetPublicShops);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopMembers>>>
    GetMembersOfTheShop
    (
        Guid shopId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyGetMembersOfTheShop.Replace("{shopId}", shopId.ToString()));

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopMembers>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopMembers>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopMembers>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopMembers>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }


    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>>
    GetShopsSubjectForModeration()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyGetShopsForModeration);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
    #endregion

    #region Post
    public async Task<ClientUtil_ApiResponse<ClientDto_Shop>>
    CreateShop
    (
        ClientDto_CreateShop createShopDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            Dictionary<string, string>? registerData = new()
            {
                { nameof(ClientDto_CreateShop.Name), createShopDto.Name },
                { nameof(ClientDto_CreateShop.Description), createShopDto.Description },
                { nameof(ClientDto_CreateShop.EncryptedSymmetricKey), createShopDto.EncryptedSymmetricKey },
                { nameof(ClientDto_CreateShop.EncryptedKeyNonce), createShopDto.EncryptedKeyNonce },
                { nameof(ClientDto_CreateShop.DataNonce), createShopDto.DataNonce },
                { nameof(ClientDto_CreateShop.SigningPublicKey), createShopDto.SigningPublicKey },
                { nameof(ClientDto_CreateShop.SignatureOfKey), createShopDto.SignatureOfKey },
                { nameof(ClientDto_CreateShop.MakePublic), createShopDto.MakePublic.ToString() },
            };

            FormUrlEncodedContent? contentForm = new(registerData);

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyCreateShop)
            {
                Content = contentForm
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Shop>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Shop>>
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
                return new ClientUtil_ApiResponse<ClientDto_Shop>()
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
            return new ClientUtil_ApiResponse<ClientDto_Shop>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    InviteUserToShop
    (
        Guid shopId,
        ClientDto_InviteUserToShop inviteUserToShopDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(inviteUserToShopDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyInviteUserToShop.Replace("{shopId}", shopId.ToString()), content);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<bool>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
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
                    Value = false
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ModerateShop
    (
       ClientDto_ModerateShop moderateShop
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(moderateShop);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyModerateShop, content);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<bool>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
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
                    Value = false
                };
            }
            else
                throw;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }
    #endregion

    #region Put
    public async Task<ClientUtil_ApiResponse<ClientDto_Shop>>
    EditShop
    (
        Guid shopId,
        ClientDto_UpdateShop editShopDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            Dictionary<string, string>? registerData = new()
            {
                { nameof(ClientDto_UpdateShop.Name), editShopDto.Name },
                { nameof(ClientDto_UpdateShop.Description), editShopDto.Description },
                { nameof(ClientDto_UpdateShop.EncryptedKeyNonce), editShopDto.EncryptedKeyNonce },
                { nameof(ClientDto_UpdateShop.DataNonce), editShopDto.DataNonce },
                { nameof(ClientDto_UpdateShop.EncryptedSymmetricKey), editShopDto.EncryptedSymmetricKey },
                { nameof(ClientDto_UpdateShop.SigningPublicKey), editShopDto.SigningPublicKey },
                { nameof(ClientDto_UpdateShop.SignatureOfKey), editShopDto.SignatureOfKey },
            };

            FormUrlEncodedContent? contentForm = new(registerData);

            HttpRequestMessage? requestMessage = new(HttpMethod.Put, BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyEditShop.Replace("{shopId}", shopId.ToString()))
            {
                Content = contentForm
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Shop>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Shop>>
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
                return new ClientUtil_ApiResponse<ClientDto_Shop>()
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
            return new ClientUtil_ApiResponse<ClientDto_Shop>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
    #endregion

    #region Delete
    public async Task<ClientUtil_ApiResponse<bool>>
    DeleteShop
    (
        Guid shopId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyDeleteShop.Replace("{shopId}", shopId.ToString()));

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<bool>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>
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
                return new ClientUtil_ApiResponse<bool>()
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
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    RemoveUserFromShop
    (
        ClientDto_RemoveUserFromShop clientDto_RemoveUserFromShop
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(clientDto_RemoveUserFromShop);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, BaseRoute.RouteShopProxy + Route_ShopRoutes.ProxyRemoveUserFromShop)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<bool>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>
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
                return new ClientUtil_ApiResponse<bool>()
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
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }
    #endregion
}