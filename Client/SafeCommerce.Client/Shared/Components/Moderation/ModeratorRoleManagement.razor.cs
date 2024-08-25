using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Moderation;

public partial class ModeratorRoleManagement
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private List<ClientDto_ShopForModeration> ShopsForModerations { get; set; } = [];
    private List<ClientDto_ItemForModeration> ItemsForModerations { get; set; } = [];

    protected override async Task 
    OnInitializedAsync()
    {
        if (await UserUtilities.GetRole(LocalStorageService) == Role.Moderator)
        {
            var getShopsForModeration = await ShopService.GetShopsSubjectForModeration();

            if (getShopsForModeration.Succsess)
                ShopsForModerations = getShopsForModeration.Value!.ToList();
            else
                _snackbar.Add(getShopsForModeration.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });

            var getItemsForModeration = await ItemService.GetItemsSubjectForModeration();

            if (getItemsForModeration.Succsess)
                ItemsForModerations = getItemsForModeration.Value!.ToList();
            else
                _snackbar.Add(getItemsForModeration.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });
        }
    }

    private async Task
    ToggleOpenDialogModration
    (
        bool approved,
        ClientDto_ShopForModeration? dto_ShopForModeration = null,
        ClientDto_ItemForModeration? clientDto_ItemForModeration = null
    )
    {
        if (dto_ShopForModeration is not null)
        {
            var parameters = new DialogParameters
            {
                { "IsApproved", approved },
                { "Shop", dto_ShopForModeration },
                { "OnShopModerated", EventCallback.Factory.Create(this, (ClientDto_ShopForModeration shop) => OnShopModerated(shop)) }
            };

            var dialog = await DialogService.ShowAsync<ModerateShopConfirmation>("", parameters, DialogHelper.SimpleDialogOptions());
            await dialog.Result;
        }
        else
        {
            var parameters = new DialogParameters
                {
                    { "IsApproved", approved },
                    { "Item", clientDto_ItemForModeration },
                    { "OnItemModerated", EventCallback.Factory.Create(this, (ClientDto_ItemForModeration item) => OnItemModerated(item)) }
                };

            var dialog = await DialogService.ShowAsync<ModerateItemConfirmation>("", parameters, DialogHelper.SimpleDialogOptions());
            await dialog.Result;
        }
    }

    private void 
    OnShopModerated
    (
        ClientDto_ShopForModeration shop
    )
    {
        ShopsForModerations.Remove(shop);
    }

    private void 
    OnItemModerated
    (
        ClientDto_ItemForModeration item
    )
    {
        ItemsForModerations.Remove(item);
    }
}