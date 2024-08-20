using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class PublicSharedShops
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private List<ClientDto_Shop> PublicShops { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getPublicShopsResult = await ShopService.GetPublicSharedShops();

        if (getPublicShopsResult.Succsess)
            PublicShops = getPublicShopsResult.Value!.ToList();
    }
}