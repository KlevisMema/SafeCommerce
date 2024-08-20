using Microsoft.AspNetCore.Http;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_UserManagment
{
    Task<ClientUtil_ApiResponse<bool>>
    ActivateAccountRequest
    (
        ClienDto_ActivateAccountRequest ActivateAccountRequest
    );


    Task<ClientUtil_ApiResponse<bool>>
    ActivateAccountRequestConfirmation
    (
        ClientDto_ActivateAccountConfirmation activateAccountConfirmation
    );


    Task<ClientUtil_ApiResponse<bool>>
    ChangePassword
    (
        ClientDto_UserChangePassword userChangePassword
    );


    Task<ClientUtil_ApiResponse<bool>>
    ConfirmChangeEmailAddressRequest
    (
        ClientDto_ChangeEmailAddressRequestConfirm changeEmailAddressRequestConfirm
    );


    Task<ClientUtil_ApiResponse<bool>>
    DeactivateAccount
    (
        ClientDto_DeactivateAccount deactivateAccount
    );


    Task<ClientUtil_ApiResponse<bool>>
    ForgotPassword
    (
        ClientDto_ForgotPassword forgotPassword
    );


    Task<ClientUtil_ApiResponse<ClientDto_UserInfo>>
    GetUser();


    Task<ClientUtil_ApiResponse<bool>>
    RequestChangeEmail
    (
        ClientDto_ChangeEmailAddressRequest changeEmailAddressRequest
    );


    Task<ClientUtil_ApiResponse<bool>>
    ResetPassword
    (
        ClientDto_ResetPassword resetPassword
    );


    Task<ClientUtil_ApiResponse<List<ClientDto_UserSearched>>>
    SearchUserByUserName
    (
        string userName,
        CancellationToken cancellationToken
    );


    Task<ClientUtil_ApiResponse<ClientDto_UserInfo>>
    UpdateUser
    (
        ClientDto_UpdateUser clientDto_UpdateUser
    );

    Task<ClientUtil_ApiResponse<byte[]>>
    UploadProfilePicture
    (
        string fileName,
        StreamContent streamContent
    );
}