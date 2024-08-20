#region Usings
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ProxyApi.Container.Interfaces;
using SafeCommerce.DataTransormObject.UserManagment;
#endregion

namespace SafeCommerce.ProxyApi.Container.Services;

public class AccountManagmentProxyService
(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AccountManagmentProxyService> logger,
    IOptions<API_Helper_RequestHeaderSettings> requestHeaderOptions,
    IRequestConfigurationProxyService requestConfigurationProxyService
) : IAccountManagmentProxyService
{
    #region Get
    public async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    GetUser
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
                    (nameof(userId), userId),
                    (nameof(userIp), userIp),
                    (nameof(jwtToken), jwtToken)
                );

                HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

                StringContent? content = new(JsonSerializer.Serialize(new { userId }), Encoding.UTF8, "application/json");

                HttpRequestMessage? requestMessage = new
                (
                    HttpMethod.Get,
                    BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.GetUser.Replace("{userId}", userId.ToString())
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

                Util_GenericResponse<DTO_UserUpdatedInfo>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_UserUpdatedInfo>>
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
                    return new Util_GenericResponse<DTO_UserUpdatedInfo>()
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
                logger.LogCritical(ex, "Exception in GetUser.");

                return new Util_GenericResponse<DTO_UserUpdatedInfo>
                {
                    Errors = null,
                    Message = ClientUtil_ExceptionResponse.GeneralMessage,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = null
                };
            }
        }

    public async Task<Util_GenericResponse<List<DTO_UserSearched>>>
    SearchUserByUserName
    (
        string userId,
        string userIp,
        string jwtToken,
        string userName,
        CancellationToken cancellationToken
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

            StringContent? content = new(JsonSerializer.Serialize(new { userName }), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Get,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.SearchUserByUserName
                                                                                      .Replace("{userId}", userId.ToString()) + $"?username={userName}"
            )
            {
                Content = content,

            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                jwtToken,
                requestMessage,
                new KeyValuePair<string, string>(requestHeaderOptions.Value.ClientIP, userIp)
            );

            response = await httpClient.SendAsync(requestMessage, cancellationToken);

            string? responseContent = await response.Content.ReadAsStringAsync();

            Util_GenericResponse<List<DTO_UserSearched>>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<List<DTO_UserSearched>>>
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
                return new Util_GenericResponse<List<DTO_UserSearched>>()
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
            logger.LogCritical(ex, "Exception in SearchUserByUserName.");

            return new Util_GenericResponse<List<DTO_UserSearched>>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
    #endregion

    #region Put
    public async Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    UpdateUser
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_UserInfo userInfo
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
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

            var registerData = new Dictionary<string, string>
            {
                { nameof(DTO_UserInfo.FullName), userInfo.FullName },
                { nameof(DTO_UserInfo.UserName), userInfo.UserName },
                { nameof(DTO_UserInfo.Gender), userInfo.Gender.ToString() },
                { nameof(DTO_UserInfo.Birthday), userInfo.Birthday.ToString() },
                { nameof(DTO_UserInfo.PhoneNumber), userInfo.PhoneNumber },
                { nameof(DTO_UserInfo.Enable2FA), userInfo.Enable2FA.ToString() },
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Put,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.UpdateUser.Replace("{userId}", userId)
            )
            {
                Content = contentForm
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

            Util_GenericResponse<DTO_UserUpdatedInfo>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<DTO_UserUpdatedInfo>>
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
                return new Util_GenericResponse<DTO_UserUpdatedInfo>()
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
            logger.LogCritical(ex, "Exception in UpdateUser.");

            return new Util_GenericResponse<DTO_UserUpdatedInfo>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
    #endregion

    #region Post
    public async Task<Util_GenericResponse<bool>>
    ChangePassword
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_UserChangePassword changePassword
    )
        {
            HttpResponseMessage response = new();

            try
            {
                API_Helper_ParamsStringChecking.CheckNullOrEmpty
                (
                    (nameof(userId), userId),
                    (nameof(userIp), userIp),
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

                var registerData = new Dictionary<string, string>
                {
                    { nameof(DTO_UserChangePassword.OldPassword), changePassword.OldPassword },
                    { nameof(DTO_UserChangePassword.NewPassword), changePassword.NewPassword },
                    { nameof(DTO_UserChangePassword.ConfirmNewPassword), changePassword.ConfirmNewPassword},
                };

                var contentForm = new FormUrlEncodedContent(registerData);

                HttpRequestMessage? requestMessage = new
                (
                    HttpMethod.Put,
                    BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ChangePassword.Replace("{userId}", userId)
                )
                {
                    Content = contentForm
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
                logger.LogCritical(ex, "Exception in ChangePassword.");

                return new Util_GenericResponse<bool>
                {
                    Errors = null,
                    Message = ClientUtil_ExceptionResponse.GeneralMessage,
                    StatusCode = response.StatusCode,
                    Succsess = false,
                    Value = false
                };
            }
        }

    public async Task<Util_GenericResponse<bool>>
    DeactivateAccount
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_DeactivateAccount deactivateAccount
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
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

            var registerData = new Dictionary<string, string>
            {
                { nameof(DTO_DeactivateAccount.Email), deactivateAccount.Email },
                { nameof(DTO_DeactivateAccount.Password), deactivateAccount.Password },
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.DeactivateAccount.Replace("{userId}", userId)
            )
            {
                Content = contentForm
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
            logger.LogCritical(ex, "Exception in DeactivateAccount.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ActivateAccountRequest
    (
        string email,
        string userIp
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            StringContent? content = new(JsonSerializer.Serialize(new { email }), Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ActivateAccountRequest + $"?email={email}"
            )
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                string.Empty,
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

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

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
            logger.LogCritical(ex, "Exception in ActivateAccountRequest.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ActivateAccountRequestConfirmation
    (
        string userIp,
        DTO_ActivateAccountConfirmation activateAccountConfirmationDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            var json = JsonSerializer.Serialize(activateAccountConfirmationDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ActivateAccountRequestConfirmation
            )
            {
                Content = content
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                string.Empty,
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

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

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
            logger.LogCritical(ex, "Exception in ActivateAccountRequestConfirmation.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ForgotPassword
    (
        string userIp,
        DTO_ForgotPassword forgotPassword
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            var registerData = new Dictionary<string, string>
            {
                { nameof(DTO_ForgotPassword.Email), forgotPassword.Email }
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ForgotPassword
            )
            {
                Content = contentForm
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                string.Empty,
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

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

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
            logger.LogCritical(ex, "Exception in ForgotPassword.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ResetPassword
    (
        string userIp,
        DTO_ResetPassword resetPassword
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userIp), userIp)
            );

            HttpClient? httpClient = API_Helper_HttpClient.CreateClientInstance(requestConfigurationProxyService.GetBaseAddrOfMainApi());

            var registerData = new Dictionary<string, string>
            {
                { nameof(DTO_ResetPassword.Email), resetPassword.Email },
                { nameof(DTO_ResetPassword.NewPassword), resetPassword.NewPassword },
                { nameof(DTO_ResetPassword.Token), resetPassword.Token },
                { nameof(DTO_ResetPassword.ConfirmNewPassword), resetPassword.ConfirmNewPassword },
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ResetPassword
            )
            {
                Content = contentForm
            };

            API_Helper_HttpClient.AddHeadersToTheRequest
            (
                string.Empty,
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

            API_Helper_CookieHelper.StoreCookies(response, httpContextAccessor);

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
            logger.LogCritical(ex, "Exception in ResetPassword.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    RequestChangeEmail
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_ChangeEmailAddressRequest emailAddress
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
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

            var registerData = new Dictionary<string, string>
            {
                { nameof(DTO_ChangeEmailAddressRequest.ConfirmNewEmailAddress), emailAddress.ConfirmNewEmailAddress },
                { nameof(DTO_ChangeEmailAddressRequest.NewEmailAddress), emailAddress.NewEmailAddress },
                { nameof(DTO_ChangeEmailAddressRequest.CurrentEmailAddress), emailAddress.CurrentEmailAddress }
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.RequestChangeEmail.Replace("{userId}", userId)
            )
            {
                Content = contentForm
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
            logger.LogCritical(ex, "Exception in RequestChangeEmail.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ConfirmChangeEmailAddressRequest
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
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

            var json = JsonSerializer.Serialize(changeEmailAddressConfirmDto);

            StringContent? content = new(json, Encoding.UTF8, "application/json");

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.ConfirmChangeEmailRequest.Replace("{userId}", userId)
            )
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
            logger.LogCritical(ex, "Exception in ConfirmChangeEmailAddressRequest.");

            return new Util_GenericResponse<bool>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<Util_GenericResponse<byte[]>>
    UploadProfilePicture
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        IFormFile image
    )
    {
        HttpResponseMessage response = new();

        try
        {
            API_Helper_ParamsStringChecking.CheckNullOrEmpty
            (
                (nameof(userId), userId),
                (nameof(userIp), userIp),
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

            var fileContent = new StreamContent(image.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(image.ContentType);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(userId), "userId" },
                { fileContent, "image", image.FileName }
            };

            HttpRequestMessage? requestMessage = new
            (
                HttpMethod.Post,
                BaseRoute.RouteAccountManagmentForClient + Route_AccountManagmentRoute.UploadProfilePicture.Replace("{userId}", userId.ToString())
            )
            {
                Content = formData
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

            Util_GenericResponse<byte[]>? readResult = JsonSerializer.Deserialize<Util_GenericResponse<byte[]>>
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
                return new Util_GenericResponse<byte[]>()
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
            logger.LogCritical(ex, "Exception in UploadProfilePicture.");

            return new Util_GenericResponse<byte[]>
            {
                Errors = null,
                Message = ClientUtil_ExceptionResponse.GeneralMessage,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    } 
    #endregion
}