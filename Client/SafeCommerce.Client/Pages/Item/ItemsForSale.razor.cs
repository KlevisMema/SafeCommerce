using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Item;

public partial class ItemsForSale
{
    [Parameter] public Guid ShopId { get; set; }

    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_Item> ItemsOfShop { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var items = await ItemService.GetItemsByShopId(ShopId);

        if (items != null && items.Succsess && items.Value is not null)
        {
            foreach (var item in items.Value)
            {
                if (item.MakePublic && item.IsApproved && item.IsPublic)
                    this.ItemsOfShop.Add(item);
                else
                {
                    string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

                    if (userId == null)
                        await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);

                    var isOwnerValid = await this.JsRuntime.InvokeAsync<bool>("verifyPublicKey", item.OwnerPublicKey, item.OwnerSigningPublicKey, item.OwnerSignature);

                    if (isOwnerValid)
                    {
                        if (!item.MakePublic)
                        {
                            ClientDto_Item decryptedItem = new();

                            if (userId == item.OwnerId.ToString())
                            {
                                decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_Item>("decryptMyItemData", item, userId);
                                decryptedItem.Price = decimal.Parse(decryptedItem.PriceDecrypted);
                            }
                            else
                                decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_Item>("decryptItemData", item, userId);

                            this.ItemsOfShop.Add(decryptedItem);
                            continue;
                        }
                    }

                }
            }
        }
    }
}