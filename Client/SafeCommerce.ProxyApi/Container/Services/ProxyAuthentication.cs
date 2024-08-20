#region Usings
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.DataTransormObject.UserManagment;

#endregion

namespace SafeCommerce.ProxyApi.Container.Services;

public class ProxyAuthentication
(
    ILogger<ProxyAuthentication> logger,
    IHttpContextAccessor httpContextAccessor,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IProxyAuthentication
{
    public async Task<Util_GenericResponse<bool>>
    RegisterUser
    (
        DTO_Register register
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            Dictionary<string, string> registerData = new()
            {
                { nameof(DTO_Register.FullName), register.FullName },
                { nameof(DTO_Register.UserName), register.UserName },
                { nameof(DTO_Register.Email), register.Email },
                { nameof(DTO_Register.Gender), register.Gender.ToString() },
                { nameof(DTO_Register.Birthday), register.Birthday.ToString() },
                { nameof(DTO_Register.PhoneNumber), register.PhoneNumber },
                { nameof(DTO_Register.Password), register.Password },
                { nameof(DTO_Register.ConfirmPassword), register.ConfirmPassword },
                { nameof(DTO_Register.Enable2FA), register.Enable2FA.ToString() },
            };

            FormUrlEncodedContent? contentForm = new(registerData);

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.Register)
            {
                Content = contentForm
            };

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
                logger.LogCritical(argException, "Exception in RegisterUser.");

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
            logger.LogCritical(ex, "Exception in RegisterUser.");

            return new Util_GenericResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ConfirmRegistration
    (
         DTO_ConfirmRegistration confirmRegistrationDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            string? json = JsonSerializer.Serialize(confirmRegistrationDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.ConfirmRegistration)
            {
                Content = content
            };

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
                logger.LogCritical(argException, "Exception in ConfirmRegistration.");

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
            logger.LogCritical(ex, "Exception in ConfirmRegistration.");

            return new Util_GenericResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ReConfirmRegistrationRequest
    (
        DTO_ReConfirmRegistration ReConfirmRegistration
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            string? json = JsonSerializer.Serialize(ReConfirmRegistration);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.ReConfirmRegistrationRequest)
            {
                Content = content
            };

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

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                logger.LogCritical(argException, "Exception in ReConfirmRegistrationRequest.");

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
            logger.LogCritical(ex, "Exception in ReConfirmRegistrationRequest.");

            return new Util_GenericResponse<bool>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<DTO_LoginResult>>
    LogIn
    (
        DTO_Login loginDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            Dictionary<string, string>? loginData = new()
            {
                { nameof(DTO_Login.Email), loginDto.Email },
                { nameof(DTO_Login.Password), loginDto.Password }
            };

            FormUrlEncodedContent? contentForm = new(loginData);

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.Login)
            {
                Content = contentForm
            };

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<DTO_LoginResult>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_LoginResult>>
            (
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                logger.LogCritical(argException, "Exception in LogIn.");

                return new Util_GenericResponse<DTO_LoginResult>()
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
            logger.LogCritical(ex, "Exception in LogIn.");

            return new Util_GenericResponse<DTO_LoginResult>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<Util_GenericResponse<string>>
    SaveUserPublicKey
    (
        string userId,
        string userIp,
        string jwtToken,
        DTO_SavePublicKey publicKey
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
                (nameof(jwtToken), jwtToken)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            StringContent? content = new(JsonSerializer.Serialize(publicKey), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post, 
                BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.SaveUserPublicKey.Replace("{userId}", userId)
            )
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

            Util_GenericResponse<string>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<string>>
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
                logger.LogCritical(argException, "Exception in SaveUserPublicKey.");

                return new Util_GenericResponse<string>()
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
            logger.LogCritical(ex, "Exception in SaveUserPublicKey.");

            return new Util_GenericResponse<string>()
            {
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<Util_GenericResponse<DTO_LoginResult>>
    ConfirmLogin2FA
    (
        Guid userId,
        string jwtToken,
        DTO_ConfirmLogin confirmLogin
    )
    {
        HttpResponseMessage response = new();

        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            string? json = JsonSerializer.Serialize(confirmLogin);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.ConfirmLogin.Replace("{userId}", userId.ToString()))
            {
                Content = content
            };

            httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);

            response = await httpClient.SendAsync(requestMessage);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<DTO_LoginResult>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_LoginResult>>
            (
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                logger.LogCritical(argException, "Exception in SaveUserPublicKey.");

                return new Util_GenericResponse<DTO_LoginResult>()
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
            logger.LogCritical(ex, "Exception in ConfirmLogin2FA.");

            return new Util_GenericResponse<DTO_LoginResult>()
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Succsess = false,
                Value = null,
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<HttpResponseMessage>
    LogoutUser
    (
        Guid userId,
        string jwtToken
    )
    {
        try
        {
            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            StringContent? content = new(JsonSerializer.Serialize(new { userId }), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.LogOut.Replace("{userId}", userId.ToString())
            )
            {
                Content = content
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            HttpResponseMessage? response = await httpClient.SendAsync(requestMessage);

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Exception in LogoutUser.");

            return new HttpResponseMessage();
        }
    }

    public async Task<Util_GenericResponse<DTO_Token>>
    RefreshToken
    (
        string jwtToken,
        string refreshToken,
        string refreshTokenId
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, BaseRoute.RouteAuthenticationForClient + Route_AuthenticationRoute.RefreshToken)
            {
                Content = null
            };

            requestMessage.Headers.Add("Cookie", $"AuthToken={jwtToken}; RefreshAuthToken={refreshToken}; RefreshAuthTokenId={refreshTokenId}");

            response = await httpClient.SendAsync(requestMessage);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_Token>>
            (
                responseContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            ) ?? throw new ArgumentNullException(ClientUtil_ExceptionResponse.ArgumentNullException);

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

            return readResult;
        }
        catch (ArgumentNullException argException)
        {
            if (argException.Message.Equals(ClientUtil_ExceptionResponse.ArgumentNullException))
            {
                logger.LogCritical(argException, "Exception in RefreshToken.");

                return new Util_GenericResponse<DTO_Token>()
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
            logger.LogCritical(ex, "Exception in RefreshToken.");

            return new Util_GenericResponse<DTO_Token>()
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                Succsess = false,
                Value = null,
                StatusCode = response.StatusCode
            };
        }
    }
}