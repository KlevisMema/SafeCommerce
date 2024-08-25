using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.Item;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class PublicSharedShops
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    private IEnumerable<ClientDto_Shop> PublicShops { get; set; } = [];
    private IEnumerable<ClientDto_PublicItem> PublicItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getPublicShopsResult = await ShopService.GetPublicSharedShops();

        if (getPublicShopsResult.Succsess && getPublicShopsResult.Value is not null)
            PublicShops = getPublicShopsResult.Value;

        var getPublicItemsResult = await ItemService.GetPublicSharedItems();

        if (getPublicItemsResult.Succsess && getPublicItemsResult.Value is not null)
            PublicItems = getPublicItemsResult.Value;
    }
}