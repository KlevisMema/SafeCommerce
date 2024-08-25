using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Item;

public partial class Items
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_Item> ListItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getListItems = await ItemService.GetUserItems();

        if (getListItems != null && getListItems.Succsess && getListItems.Value is not null)
        {
            string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

            if (userId == null)
                await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);

            foreach (var item in getListItems.Value)
            {
                bool isValidCrypoKey = await this.JsRuntime.InvokeAsync<bool>("verifyPublicKey", item.OwnerPublicKey, item.OwnerSigningPublicKey, item.OwnerSignature);

                if (isValidCrypoKey)
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
                        {
                            decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_Item>("decryptItemData", item, userId);
                            decryptedItem.Price = decimal.Parse(decryptedItem.PriceDecrypted);
                        }

                        this.ListItems.Add(decryptedItem);
                        continue;
                    }
                    this.ListItems.Add(item);
                }
            }
        }
    }
}