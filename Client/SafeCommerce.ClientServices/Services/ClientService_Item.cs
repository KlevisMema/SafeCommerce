#region Usings
using System.Text;
using System.Text.Json;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientDTO.Moderation;
using System.Net.Http.Json;
using SafeCommerce.ClientDTO.Shop;
#endregion

namespace SafeCommerce.ClientServices.Services;

public class ClientService_Item
(
    IHttpClientFactory httpClientFactory
) : IClientService_Item
{
    #region Get
    public async Task<ClientUtil_ApiResponse<ClientDto_Item>>
    GetItemDetails
    (
        Guid itemId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyGetItemDetails.Replace("{itemId}", itemId.ToString()));

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Item>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Item>>
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
                return new ClientUtil_ApiResponse<ClientDto_Item>()
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
            return new ClientUtil_ApiResponse<ClientDto_Item>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<List<ClientDto_Item>>>
    GetItemsByShopId
    (
        Guid shopId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyGetItemsByShopId.Replace("{shopId}", shopId.ToString()));

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<List<ClientDto_Item>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<List<ClientDto_Item>>>
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
                return new ClientUtil_ApiResponse<List<ClientDto_Item>>()
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
            return new ClientUtil_ApiResponse<List<ClientDto_Item>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>>
    GetUserItems()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyGetUserItems);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>>
    GetPublicSharedItems()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyGetPublicSharedItems);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>>
    GetItemsSubjectForModeration()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyGetItemsForModeration);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>>
    GetMembersOfTheItem
    (
        Guid itemId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ShopRoutes.ProxyGetMembersOfTheItem.Replace("{itemId}", itemId.ToString()));

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>>
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
                return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>()
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
            return new ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>()
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
    public async Task<ClientUtil_ApiResponse<ClientDto_Item>>
    CreateItem
    (
        ClientDto_CreateItem createItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            StringContent content = new(JsonSerializer.Serialize(createItemDto), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyCreateItem)
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Item>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Item>>
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
                return new ClientUtil_ApiResponse<ClientDto_Item>()
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
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_Item>()
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
    ShareItem
    (
        ClientDto_ShareItem shareItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(shareItemDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyShareItem, content);

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
    ModerateItem
    (
        ClientDto_ModerateItem moderateItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(moderateItemDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyModerateItem, content);

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

    #region Put
    public async Task<ClientUtil_ApiResponse<ClientDto_Item>>
    EditItem
    (
        Guid itemId,
        ClientDto_UpdateItem editItemDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            StringContent content = new(JsonSerializer.Serialize(editItemDto), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Put, BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyEditItem.Replace("{itemId}", itemId.ToString()))
            {
                Content = content
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            ClientUtil_ApiResponse<ClientDto_Item>? readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_Item>>
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
                return new ClientUtil_ApiResponse<ClientDto_Item>()
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
            return new ClientUtil_ApiResponse<ClientDto_Item>()
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
    DeleteItem
    (
        Guid itemId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyDeleteItem.Replace("{itemId}", itemId.ToString()));

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
    RemoveUserFromItem
    (
        ClientDto_RemoveUserFromItem clientDto_RemoveUserFromItem
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            string? json = JsonSerializer.Serialize(clientDto_RemoveUserFromItem);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Delete, BaseRoute.RouteItemProxy + Route_ShopRoutes.ProxyRemoveUserFromItem)
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