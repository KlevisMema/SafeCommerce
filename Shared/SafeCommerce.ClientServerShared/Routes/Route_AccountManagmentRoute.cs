/* 
 * Contains route definitions for various aspects of the SafeCommerce client-server communication.
 */

namespace SafeCommerce.ClientServerShared.Routes;

/// <summary>
/// Routes related to account management functionalities.
/// </summary>
public static class Route_AccountManagmentRoute
{
    public const string GetUser = "GetUser/{userId}";
    public const string ResetPassword = "ResetPassword";
    public const string ForgotPassword = "ForgotPassword";
    public const string UpdateUser = "UpdateUser/{userId}";
    public const string ChangePassword = "ChangePassword/{userId}";
    public const string DeactivateAccount = "DeactivateAccount/{userId}";
    public const string ActivateAccountRequest = "ActivateAccountRequest";
    public const string RequestChangeEmail = "RequestChangeEmail/{userId}";
    public const string SearchUserByUserName = "SearchUserByUserName/{userId}";
    public const string UploadProfilePicture = "UploadProfilePicture/{userId}";
    public const string ConfirmChangeEmailRequest = "ConfirmChangeEmailRequest/{userId}";
    public const string ActivateAccountRequestConfirmation = "ActivateAccountRequestConfirmation";

    public const string ProxyGetUser = "GetUser";
    public const string ProxyUpdateUser = "UpdateUser";
    public const string ProxyChangePassword = "ChangePassword";
    public const string ProxyDeactivateAccount = "DeactivateAccount";
    public const string ProxyRequestChangeEmail = "RequestChangeEmail";
    public const string ProxySearchUserByUserName = "SearchUserByUserName";
    public const string ProxyUploadProfilePicture = "UploadProfilePicture";
    public const string ProxyConfirmChangeEmailRequest = "ConfirmChangeEmailRequest";
}