using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.Client.Internal;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class PublicSharedShops
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ShoppingCartService ShoppingCartService { get; set; } = null!;

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

    private async Task
    AddItemInShoppingBag
    (
        ClientDto_PublicItem item
    )
    {
        try
        {
            await ShoppingCartService.AddItemAsync(new ClientDto_CartItem
            {
                ItemId = item.ItemId,
                ImageUrl = item.Picture,
                Name = item.Name,
                Price = item.Price.Value,
                Quantity = 1
            });

            _snackbar.Add("Item added in the shopping bag.", Severity.Success, config => { config.CloseAfterNavigation = true; });
        }
        catch (Exception)
        {
            _snackbar.Add("Item was not added in shopping bag!", Severity.Error, config => { config.CloseAfterNavigation = true; });
        }
    }
}