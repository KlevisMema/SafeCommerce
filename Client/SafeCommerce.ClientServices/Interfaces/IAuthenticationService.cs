using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientDTO.User;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IAuthenticationService
{
    Task<ClientUtil_ApiResponse<bool>>
    RegisterUser
    (
        ClientDto_Register register
    );

    Task<ClientUtil_ApiResponse<ClientDto_LoginResult>>
    LogInUser
    (
        ClientDto_Login login
    );

    Task
    LogoutUser();

    Task<string>
    GetJwtToken();

    Task<ClientUtil_ApiResponse<bool>>
    ConfirmUserRegistration
    (
        ClientDto_ConfirmRegistration confirmRegistration
    );

    Task<ClientUtil_ApiResponse<bool>>
    ReConfirmRegistrationRequest
    (
        ClientDto_ReConfirmRegistration ConfirmRegistration
    );

    Task<ClientUtil_ApiResponse<ClientDto_LoginResult>>
    ConfirmLogin2FA
    (
        ClientDto_2FA TwoFA
    );

    Task<ClientUtil_ApiResponse<string>>
    SaveUserPublicKey
    (
        string userId,
        ClientDto_SavePublicKey publicKey
    );
}