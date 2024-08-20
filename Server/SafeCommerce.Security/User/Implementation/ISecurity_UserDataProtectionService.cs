namespace SafeCommerce.Security.User.Implementation;

public interface ISecurity_UserDataProtectionService
{
    string
    Protect
    (
        string data,
        string userId
    );

    string
    Unprotect
    (
        string protectedData,
        string userId
    );
}