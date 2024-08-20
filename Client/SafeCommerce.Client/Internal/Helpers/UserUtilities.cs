using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.Client.Internal.Helpers;

internal static class UserUtilities
{
    internal static async Task<Role> 
    GetRole
    (
        ILocalStorageService LocalStorageService
    )
    {
        string? userRole = await LocalStorageService.GetItemAsStringAsync("Role");

        if (String.IsNullOrEmpty(userRole))
            return Role.NoRole;

        if (userRole == Role.User.ToString())
            return Role.User;

        return Role.Moderator;
    }
}