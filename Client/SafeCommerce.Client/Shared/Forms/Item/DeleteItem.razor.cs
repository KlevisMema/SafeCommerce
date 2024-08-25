using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Item;

public partial class DeleteItem
{
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public ClientDto_Item ItemToBeDeleted { get; set; }
    [Parameter] public bool PrivateItem { get; set; }
    [Parameter] public EventCallback OnItemDeleted { get; set; }

    private bool _processing = false;

    private async Task
    OnDeleteShop()
    {
        _processing = true;
        await Task.Delay(1000);

        var deleteShopResult = await ItemService.DeleteItem(ItemToBeDeleted.ItemId);

        if (deleteShopResult.Succsess)
        {
            if (PrivateItem)
                await JsRuntime.InvokeVoidAsync("deleteItemConfirm", ItemToBeDeleted.ItemId);

            await OnItemDeleted.InvokeAsync();
        }

        MudDialog.Close();
        _processing = false;
        await InvokeAsync(StateHasChanged);
    }
}