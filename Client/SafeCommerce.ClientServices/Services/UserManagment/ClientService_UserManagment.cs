using System.Text;
using System.Text.Json;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServerShared.Routes;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.ClientServices.Services.UserManagment;

public class ClientService_UserManagment(IHttpClientFactory httpClientFactory) : IClientService_UserManagment
{
    public async Task<ClientUtil_ApiResponse<ClientDto_UserInfo>>
    GetUser()
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyGetUser);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_UserInfo>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_UserInfo>
            {
                Message = "Something went wrong, user was not retrieved",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<ClientDto_UserInfo>>
    UpdateUser
    (
        ClientDto_UpdateUser clientDto_UpdateUser
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var updateUserData = new Dictionary<string, string>
            {
                { nameof(ClientDto_UpdateUser.FullName), clientDto_UpdateUser.FullName },
                { nameof(ClientDto_UpdateUser.Birthday), clientDto_UpdateUser.Birthday.ToString() },
                { nameof(ClientDto_UpdateUser.Gender), clientDto_UpdateUser.Gender.ToString() },
                { nameof(ClientDto_UpdateUser.UserName), clientDto_UpdateUser.UserName },
                { nameof(ClientDto_UpdateUser.PhoneNumber), clientDto_UpdateUser.PhoneNumber },
                { nameof(ClientDto_UpdateUser.Enable2FA), clientDto_UpdateUser.Enable2FA.ToString() }
            };

            var content = new FormUrlEncodedContent(updateUserData);

            response = await httpClient.PutAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyUpdateUser, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<ClientDto_UserInfo>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<ClientDto_UserInfo>()
            {
                Message = "Something went wrong, your data was not updated",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ChangePassword
    (
        ClientDto_UserChangePassword userChangePassword
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var changePasswordData = new Dictionary<string, string>
            {
                { nameof(ClientDto_UserChangePassword.OldPassword), userChangePassword.OldPassword },
                { nameof(ClientDto_UserChangePassword.NewPassword), userChangePassword.NewPassword},
                { nameof(ClientDto_UserChangePassword.ConfirmNewPassword), userChangePassword.ConfirmNewPassword},
            };

            var content = new FormUrlEncodedContent(changePasswordData);

            response = await httpClient.PutAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyChangePassword, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong, your password was not changed",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    DeactivateAccount
    (
        ClientDto_DeactivateAccount deactivateAccount
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var deactivateAccountData = new Dictionary<string, string>
            {
                { nameof(ClientDto_DeactivateAccount.Email), deactivateAccount.Email },
                { nameof(ClientDto_DeactivateAccount.Password), deactivateAccount.Password}
            };

            var content = new FormUrlEncodedContent(deactivateAccountData);

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyDeactivateAccount, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;

        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong, your account was not deactivated",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ActivateAccountRequest
    (
         ClienDto_ActivateAccountRequest ActivateAccountRequest
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(ActivateAccountRequest.Email);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ActivateAccountRequest + $"?email={ActivateAccountRequest.Email}", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong with your request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ActivateAccountRequestConfirmation
    (
        ClientDto_ActivateAccountConfirmation activateAccountConfirmation
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(activateAccountConfirmation);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ActivateAccountRequestConfirmation, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong, your account was not activated",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ForgotPassword
    (
        ClientDto_ForgotPassword forgotPassword
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var forgotPasswordData = new Dictionary<string, string>
            {
                { nameof(ClientDto_ForgotPassword.Email), forgotPassword.Email }
            };

            var content = new FormUrlEncodedContent(forgotPasswordData);

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ForgotPassword, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ResetPassword
    (
        ClientDto_ResetPassword resetPassword
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var requestMessage = new HttpRequestMessage();
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var forgotPasswordData = new Dictionary<string, string>
            {
                { nameof(ClientDto_ResetPassword.Email), resetPassword.Email },
                { nameof(ClientDto_ResetPassword.Token), resetPassword.Token },
                { nameof(ClientDto_ResetPassword.NewPassword), resetPassword.NewPassword },
                { nameof(ClientDto_ResetPassword.ConfirmNewPassword), resetPassword.ConfirmNewPassword }
            };

            var content = new FormUrlEncodedContent(forgotPasswordData);

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ResetPassword, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    RequestChangeEmail
    (
        ClientDto_ChangeEmailAddressRequest changeEmailAddressRequest
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var requestChangeEmailData = new Dictionary<string, string>
            {
                { nameof(ClientDto_ChangeEmailAddressRequest.ConfirmNewEmailAddress), changeEmailAddressRequest.ConfirmNewEmailAddress },
                { nameof(ClientDto_ChangeEmailAddressRequest.NewEmailAddress), changeEmailAddressRequest.NewEmailAddress },
                { nameof(ClientDto_ChangeEmailAddressRequest.CurrentEmailAddress), changeEmailAddressRequest.CurrentEmailAddress },
            };

            var content = new FormUrlEncodedContent(requestChangeEmailData);

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyRequestChangeEmail, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<bool>>
    ConfirmChangeEmailAddressRequest
    (
        ClientDto_ChangeEmailAddressRequestConfirm changeEmailAddressRequestConfirm
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var json = JsonSerializer.Serialize(changeEmailAddressRequestConfirm);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyConfirmChangeEmailRequest, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<bool>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {

            return new ClientUtil_ApiResponse<bool>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = false
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<List<ClientDto_UserSearched>>>
    SearchUserByUserName
    (
        string userName,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            response = await httpClient.GetAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxySearchUserByUserName + $"?username={userName}", cancellationToken);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<List<ClientDto_UserSearched>>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {

            return new ClientUtil_ApiResponse<List<ClientDto_UserSearched>>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }

    public async Task<ClientUtil_ApiResponse<byte[]>>
    UploadProfilePicture
    (
        string fileName,
        StreamContent streamContent
    )
    {
        HttpResponseMessage response = new();

        try
        {
            var httpClient = httpClientFactory.CreateClient(ClientUtilHelpers_Statics.HttpClientName);

            var formData = new MultipartFormDataContent
            {
                {streamContent, "image", fileName }
            };

            response = await httpClient.PostAsync(BaseRoute.RouteAccountManagmentProxy + Route_AccountManagmentRoute.ProxyUploadProfilePicture, formData);

            var responseContent = await response.Content.ReadAsStringAsync();

            var readResult = JsonSerializer.Deserialize<ClientUtil_ApiResponse<byte[]>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return readResult!;
        }
        catch (Exception)
        {
            return new ClientUtil_ApiResponse<byte[]>()
            {
                Message = "Something went wrong with the request",
                Errors = null,
                StatusCode = response.StatusCode,
                Succsess = false,
                Value = null
            };
        }
    }
}