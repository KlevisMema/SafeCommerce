using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Internal.Helpers;

internal static class LogOutHelper
{
    internal static async Task LogOut
    (
        NavigationManager navigationManager,
        ILocalStorageService localStorageService,
        IAuthenticationService authenticationService
    )
    {
        if (await localStorageService.ContainKeyAsync("SessionExpired"))
            await localStorageService.RemoveItemAsync("SessionExpired");

        if (await localStorageService.ContainKeyAsync("FullName"))
            await localStorageService.RemoveItemAsync("FullName");

        if (await localStorageService.ContainKeyAsync("Id"))
            await localStorageService.RemoveItemAsync("Id");
        
        if (await localStorageService.ContainKeyAsync("Role"))
            await localStorageService.RemoveItemAsync("Role");

        await authenticationService.LogoutUser();
        await localStorageService.RemoveItemAsync("UserData");
        navigationManager.NavigateTo("/", true);
    }
}