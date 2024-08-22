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

    public async Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>>
    GetItemsSubjectForModeration()
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteItemProxy + Route_ItemRoutes.PeoxyGetItemsForModeration);

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
    #endregion

    #region Post
    public async Task<ClientUtil_ApiResponse<bool>>
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
                    Value = false,
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

            Dictionary<string, string>? registerData = new()
            {
                { nameof(ClientDto_UpdateItem.Name), editItemDto.Name },
                { nameof(ClientDto_UpdateItem.Price), editItemDto.Price.ToString() },
                { nameof(ClientDto_UpdateItem.Picture), editItemDto.Picture!.ToString()! },
                { nameof(ClientDto_UpdateItem.Description), editItemDto.Description },
            };

            FormUrlEncodedContent? contentForm = new(registerData);

            HttpRequestMessage? requestMessage = new(HttpMethod.Put, BaseRoute.RouteItemProxy + Route_ItemRoutes.ProxyEditItem.Replace("{itemId}", itemId.ToString()))
            {
                Content = contentForm
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
    #endregion
}