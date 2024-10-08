﻿using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.Client.Internal.Helpers;

namespace SafeCommerce.Client.Pages.Shop;

public partial class Shops
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_Shop> ListShops { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getListShops = await this.ShopService.GetUserShops();

        if (getListShops != null && getListShops.Succsess && getListShops.Value is not null)
        {
            string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

            if (userId == null)
                await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);

            foreach (var shop in getListShops.Value)
            {
                if (!shop.MakePublic)
                {
                    ClientDto_Shop decryptedShop = new();

                    if (userId == shop.OwnerId.ToString())
                        decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_Shop>("decryptMyShopData", shop, userId);
                    else
                        decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_Shop>("decryptShopData", shop, userId);

                    this.ListShops.Add(decryptedShop);
                    continue;
                }

                this.ListShops.Add(shop);
            }
        }
    }
}