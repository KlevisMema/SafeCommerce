using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Authentication;
using SafeCommerce.DataTransormObject.Security;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IProxyAuthentication
{
    Task<Util_GenericResponse<bool>>
    RegisterUser
    (
        DTO_Register register
    );

    Task<Util_GenericResponse<bool>>
    ConfirmRegistration
    (
         DTO_ConfirmRegistration confirmRegistrationDto
    );

    Task<Util_GenericResponse<bool>>
    ReConfirmRegistrationRequest
    (
        DTO_ReConfirmRegistration ReConfirmRegistration
    );

    Task<Util_GenericResponse<DTO_LoginResult>>
    LogIn
    (
        DTO_Login loginDto
    );

    Task<Util_GenericResponse<DTO_LoginResult>>
    ConfirmLogin2FA
    (
        Guid userId,
        string jwtToken,
        DTO_ConfirmLogin confirmLogin
    );

    Task<HttpResponseMessage>
    LogoutUser
    (
        Guid userId,
        string jwtToken
    );

    Task<Util_GenericResponse<DTO_Token>>
    RefreshToken
    (
        string jwtToken,
        string refreshToken,
        string refreshTokenId
    );

    Task<Util_GenericResponse<string>>
    SaveUserPublicKey
    (
        string userId,
        string userIp,
        string jwtToken,
        DTO_SavePublicKey publicKey
    );
}