using Blazored.SessionStorage;
using static SafeCommerce.Client.Pages.Item.ShoppingCart;

namespace SafeCommerce.Client.Internal;

public class ShoppingCartService(ISessionStorageService sessionStorageService)
{
    private const string CartSessionKey = "shoppingCart";

    private List<CartItem> CartItems { get; set; } = new();

    public async Task InitializeCartAsync()
    {
        CartItems = await sessionStorageService.GetItemAsync<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
    }

    public IReadOnlyList<CartItem> GetCartItems() => CartItems.AsReadOnly();

    public async Task AddItemAsync(CartItem item)
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            CartItems.Add(item);
        }
        await SaveCartAsync();
    }

    public async Task RemoveItemAsync(CartItem item)
    {
        CartItems.Remove(item);
        await SaveCartAsync();
    }

    public async Task IncreaseQuantityAsync(CartItem item)
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);
        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        await SaveCartAsync();
    }

    public async Task DecreaseQuantityAsync(CartItem item)
    {
        var existingItem = CartItems.FirstOrDefault(ci => ci.Name == item.Name);
        if (existingItem != null && existingItem.Quantity > 1)
        {
            existingItem.Quantity--;
        }
        await SaveCartAsync();
    }

    public decimal GetTotalPrice()
    {
        return CartItems.Sum(item => item.TotalPrice);
    }

    private async Task SaveCartAsync()
    {
        await sessionStorageService.SetItemAsync(CartSessionKey, CartItems);
    }
}