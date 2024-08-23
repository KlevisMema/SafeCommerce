using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class ModeratorRoleShops
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private List<ClientDto_ShopForModeration> ShopsForModerations { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        if (await UserUtilities.GetRole(LocalStorageService) == Role.Moderator)
        {
            var getShopsForModeration = await ShopService.GetShopsSubjectForModeration();

            if (getShopsForModeration.Succsess)
                this.ShopsForModerations = getShopsForModeration.Value!.ToList();
            else
                _snackbar.Add(getShopsForModeration.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });
        }
    }
    private async Task
    ToggleOpenDialogModration
    (
        bool approveShop,
        ClientDto_ShopForModeration dto_ShopForModeration
    )
    {
        var parameters = new DialogParameters
        {
            { "IsApproved", approveShop },
            { "Shop", dto_ShopForModeration },
            { "OnShopModerated", EventCallback.Factory.Create(this, (ClientDto_ShopForModeration shop) => OnShopModerated(shop)) }
        };

        var dialog = await DialogService.ShowAsync<ModerateShopConfirmation>("", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private async Task OnShopModerated
    (
        ClientDto_ShopForModeration shop
    )
    {
        ShopsForModerations.Remove(shop);
    }
}