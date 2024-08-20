using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace SafeCommerce.Client.Shared.Forms.Shop;

public partial class DeleteShop
{
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public ClientDto_Shop ShopToBeDeleted { get; set; }
    [Parameter] public bool PrivateShop { get; set; }
    [Parameter] public EventCallback OnShopDeleted { get; set; }

    private bool _processing = false;

    private async Task
    OnDeleteShop()
    {
        _processing = true;
        await Task.Delay(1000);

        var deleteShopResult = await ShopService.DeleteShop(ShopToBeDeleted.ShopId);

        if (deleteShopResult.Succsess)
        {
            if (PrivateShop)
                await JsRuntime.InvokeVoidAsync("deleteShopConfirm", ShopToBeDeleted.ShopId);
            
            await OnShopDeleted.InvokeAsync();
        }

        MudDialog.Close();
        _processing = false;
        await InvokeAsync(StateHasChanged);
    }
}
