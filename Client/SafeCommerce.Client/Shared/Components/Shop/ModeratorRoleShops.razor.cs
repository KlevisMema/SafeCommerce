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
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private List<ClientDto_ShopForModeration> ShopsForModerations { get; set; } = [];
    private bool ApproveShop { get; set; }
    private bool _open;
    private bool _processing = false;

    private void ToggleOpen(bool approveShop)
    {
        this.ApproveShop = approveShop;
        this._open = !this._open;
    }

    protected override async Task OnInitializedAsync()
    {
        if (await UserUtilities.GetRole(LocalStorageService) == Role.Moderator)
        {
            var getShopsForModeration = await ShopService.GetShopsSubjectForModeration();

            if (getShopsForModeration.Succsess)
                this.ShopsForModerations = getShopsForModeration.Value!.ToList();
        }
    }

    private async Task ModerateShop
    (
        ClientDto_ShopForModeration shop
    )
    {
        _processing = true;
        await Task.Delay(1000);

        var moderaionResult = await ShopService.ModerateShop(new ClientDto_ModerateShop { Approved = ApproveShop, ShopId = shop.ShopId });

        if (moderaionResult.Succsess)
        {
            ShopsForModerations.Remove(shop);
            _snackbar.Add("Shop moderated succsessfully", Severity.Success, config => { config.CloseAfterNavigation = true; });
        }
        else
            _snackbar.Add(moderaionResult.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });

        _processing = false;
        this._open = !this._open;
    }
}