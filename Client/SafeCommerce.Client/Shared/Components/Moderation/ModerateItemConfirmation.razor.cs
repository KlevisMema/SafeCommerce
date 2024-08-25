using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Moderation;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Moderation;

public partial class ModerateItemConfirmation
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    [Parameter] public bool IsApproved { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public ClientDto_ItemForModeration Item { get; set; }
    [Parameter] public EventCallback<ClientDto_ItemForModeration> OnItemModerated { get; set; }

    private string text { get; set; } = string.Empty;
    private bool _processing = false;

    protected override Task OnInitializedAsync()
    {
        if (IsApproved)
            text = "aprove";
        else
            text = "reject";

        return base.OnInitializedAsync();
    }

    private async Task ModerateShop()
    {
        _processing = true;
        await Task.Delay(1000);

        var moderaionResult = await ItemService.ModerateItem(new ClientDto_ModerateItem { Approved = IsApproved, ItemId = Item.ItemId });

        if (moderaionResult.Succsess)
        {
            await OnItemModerated.InvokeAsync(Item);
            _snackbar.Add(moderaionResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
        }
        else
            _snackbar.Add(moderaionResult.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });

        _processing = false;
        MudDialog.Close();
    }
}