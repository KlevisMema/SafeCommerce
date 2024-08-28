using System.Text;
using System.Text.Json;
using SafeCommerce.ClientDTO.User;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.ClientServices.Authentication;

public class AuthenticationService(IHttpClientFactory httpClientFactory) : IAuthenticationService
{
    public async Task<ClientUtil_ApiResponse<bool>>
    RegisterUser
    (
         ClientDto_Register register
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var registerData = new Dictionary<string, string>
            {
                { nameof(ClientDto_Register.FullName), register.FullName },
                { nameof(ClientDto_Register.Username), register.Username },
                { nameof(ClientDto_Register.Email), register.Email },
                { nameof(ClientDto_Register.Gender), register.Gender.ToString() },
                { nameof(ClientDto_Register.Birthday), register.Birthday.ToString()! },
                { nameof(ClientDto_Register.PhoneNumber), register.PhoneNumber },
                { nameof(ClientDto_Register.Password), register.Password },
                { nameof(ClientDto_Register.ConfirmPassword), register.ConfirmPassword },
                { nameof(ClientDto_Register.Enable2FA), register.Enable2FA.ToString() },
            };

            var contentForm = new FormUrlEncodedContent(registerData);

            response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.Register, contentForm);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;

        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = "Something went wrong, could not register!",
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<ClientDto_LoginResult>>
    LogInUser
    (
        ClientDto_Login login
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var loginData = new Dictionary<string, string>
            {
                { nameof(ClientDto_Login.Email), login.Email },
                { nameof(ClientDto_Login.Password), login.Password }
            };

            var contentForm = new FormUrlEncodedContent(loginData);

            response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.Login, contentForm);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_LoginResult>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_LoginResult>()
            {
                Message = "Something went wrong, could not login!",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task
    LogoutUser
    (

    )
    {
        var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

        await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.LogOut, null);
    }

    public async Task<string>
    GetJwtToken()
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.JwtToken);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<string>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ConfirmUserRegistration
    (
        ClientDto_ConfirmRegistration confirmRegistration
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();

            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(confirmRegistration);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.ConfirmRegistration, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = "Something went wrong, could not confirm login",
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }

    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ReConfirmRegistrationRequest
    (
        ClientDto_ReConfirmRegistration ConfirmRegistration
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(ConfirmRegistration);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.ReConfirmRegistrationRequest, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>
            {
                Errors = null,
                Message = "Something went wrong, could not confirm re registration proccess",
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<ClientDto_LoginResult>>
    ConfirmLogin2FA
    (
        ClientDto_2FA TwoFA
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();

            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(TwoFA);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.ConfirmLogin.Replace("{userId}", TwoFA.UserId.ToString()), content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_LoginResult>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_LoginResult>()
            {
                Errors = null,
                Message = "Something went wrong, please try logging in again",
                Succsess = false,
                Value = null,
                StatusCode = response.StatusCode
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<string>>
    SaveUserPublicKey
    (
        string userId,
        ClientDto_SavePublicKey publicKey
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);
            var content = new StringContent(JsonSerializer.Serialize(publicKey), Encoding.UTF8, "application/json");

            var url = BaseRoute.RouteAuthenticationProxy + Route_AuthenticationRoute.SaveUserPublicKey.Replace("{userId}", userId);

            response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<string>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new ArgumentNullException("Failed to deserialize the server response. The content may not match the expected format.");

            return readResult;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<string>()
            {
                Errors = null,
                Message = "Something went wrong, please try logging in again",
                Succsess = false,
                Value = null,
                StatusCode = response.StatusCode
            };
        }
    }
}