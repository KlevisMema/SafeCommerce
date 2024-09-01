using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Internal;

public class ShoppingCartService
(
    NavigationManager navigationManager,
    IClientService_CheckOut CheckOutService,
    ILocalStorageService localStorageService,
    ISessionStorageService sessionStorageService
)
{
    private const string CartSessionKey = "shoppingCart";

    private List<ClientDto_CartItem> CartItems { get; set; } = new();

    public async Task InitializeCartAsync()
    {
        CartItems = await sessionStorageService.GetItemAsync<List<ClientDto_CartItem>> (CartSessionKey) ?? new List<ClientDto_CartItem>();
    }

    public IReadOnlyList<ClientDto_CartItem> GetCartItems() => CartItems.AsReadOnly();

    public async Task 
    AddItemAsync
    (
        ClientDto_CartItem item
    )
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);

        if (existingItem != null)
            existingItem.Quantity += item.Quantity;
        else
            CartItems.Add(item);

        await SaveCartAsync();
    }

    public async Task 
    RemoveItemAsync
    (
        ClientDto_CartItem item
    )
    {
        CartItems.Remove(item);
        await SaveCartAsync();
    }

    public async Task 
    IncreaseQuantityAsync
    (
        ClientDto_CartItem item
    )
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);

        if (existingItem != null)
            existingItem.Quantity++;

        await SaveCartAsync();
    }

    public async Task 
    DecreaseQuantityAsync
    (
        ClientDto_CartItem item
    )
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);

        if (existingItem != null && existingItem.Quantity > 1)
            existingItem.Quantity--;

        await SaveCartAsync();
    }

    public decimal 
    GetTotalPrice()
    {
        return CartItems.Sum(item => item.TotalPrice);
    }

    private async Task 
    SaveCartAsync()
    {
        await sessionStorageService.SetItemAsync(CartSessionKey, CartItems);
    }

    public async Task<string>
    CheckOut()
    {
        var userId = await localStorageService.GetItemAsStringAsync("Id");

        if (userId == null)
        {
            navigationManager.NavigateTo("/", true);
            return string.Empty;
        }

        var url = await CheckOutService.CheckOut(userId, this.CartItems);

        if (!String.IsNullOrEmpty(url))
            navigationManager.NavigateTo(url, true);

        return string.Empty;
    }
}