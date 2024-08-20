/* 
 * Contains route definitions for various aspects of the SafeCommerce client-server communication.
 */

namespace SafeCommerce.ClientServerShared.Routes;

/// <summary>
/// Routes for authentication processes.
/// </summary>
public static class Route_AuthenticationRoute
{
    public const string Login = "Login";
    public const string Register = "Register";
    public const string JwtToken = "GetJwtToken";
    public const string LogOut = "LogOut/{userId}";
    public const string RefreshToken = "RefreshToken";
    public const string VerifyPk = "VerifyPk/{userId}";
    public const string ConfirmLogin = "ConfirmLogin/{userId}";
    public const string ConfirmRegistration = "ConfirmRegistration";
    public const string SaveUserPublicKey = "SaveUserPublicKey/{userId}";
    public const string ReConfirmRegistrationRequest = "ReConfirmRegistrationRequest";
}