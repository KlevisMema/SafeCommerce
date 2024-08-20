using System.Text.Json;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.ProxyApi.Container.Services;
public class ModerationProxyService
(
    ILogger<ModerationProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IModerationProxyService
{
    public async Task<Util_GenericResponse<DTO_SplittedModerationHistory>>
    GetModerationHistoryForModerator
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

            string? requestUrl = $"{BaseRoute.RouteModeration}{Route_Moderation.GetModerationHistoryForModerator
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

            Util_GenericResponse<DTO_SplittedModerationHistory> readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_SplittedModerationHistory>>
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
                return new Util_GenericResponse<DTO_SplittedModerationHistory>()
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
            logger.LogCritical(ex, "Exception in GetModerationHistoryForModerator.");

            return new Util_GenericResponse<DTO_SplittedModerationHistory>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null,
            };
        }
    }
}