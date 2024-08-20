#region Usings
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.UserManagment;

#endregion

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IAccountManagmentProxyService
{
    #region Get
    Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    GetUser
    (
        string userId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<List<DTO_UserSearched>>>
    SearchUserByUserName
    (
        string userId,
        string userIp,
        string jwtToken,
        string userName,
        CancellationToken cancellationToken
    );
    #endregion

    #region Put
    Task<Util_GenericResponse<DTO_UserUpdatedInfo>>
    UpdateUser
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_UserInfo userInfo
    );
    #endregion

    #region Post
    Task<Util_GenericResponse<bool>>
    ChangePassword
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_UserChangePassword changePassword
    );

    Task<Util_GenericResponse<bool>>
    DeactivateAccount
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_DeactivateAccount deactivateAccount
    );

    Task<Util_GenericResponse<bool>>
    ActivateAccountRequest
    (
        string email,
        string userIp
    );

    Task<Util_GenericResponse<bool>>
    ActivateAccountRequestConfirmation
    (
        string userIp,
        DTO_ActivateAccountConfirmation activateAccountConfirmationDto
    );

    Task<Util_GenericResponse<bool>>
    ForgotPassword
    (
        string userIp,
        DTO_ForgotPassword forgotPassword
    );

    Task<Util_GenericResponse<bool>>
    ResetPassword
    (
        string userIp,
        DTO_ResetPassword resetPassword
    );

    Task<Util_GenericResponse<bool>>
    RequestChangeEmail
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_ChangeEmailAddressRequest emailAddress
    );

    Task<Util_GenericResponse<bool>>
    ConfirmChangeEmailAddressRequest
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        DTO_ChangeEmailAddressRequestConfirm changeEmailAddressConfirmDto
    );



    Task<Util_GenericResponse<byte[]>>
    UploadProfilePicture
    (
        string userId,
        string userIp,
        string jwtToken,
        string fogeryToken,
        string aspNetForgeryToken,
        IFormFile image
    ); 
    #endregion
}