using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Moderation;

public partial class ModerateShopConfirmation
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    [Parameter] public bool IsApproved { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Parameter] public ClientDto_ShopForModeration Shop { get; set; }
    [Parameter] public EventCallback<ClientDto_ShopForModeration> OnShopModerated { get; set; }

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

        var moderaionResult = await ShopService.ModerateShop(new ClientDto_ModerateShop { Approved = IsApproved, ShopId = Shop.ShopId });

        if (moderaionResult.Succsess)
        {
            await OnShopModerated.InvokeAsync(Shop);
            _snackbar.Add("Shop moderated succsessfully", Severity.Success, config => { config.CloseAfterNavigation = true; });
        }
        else
            _snackbar.Add(moderaionResult.Message, Severity.Error, config => { config.CloseAfterNavigation = true; });

        _processing = false;
        MudDialog.Close();
    }
}