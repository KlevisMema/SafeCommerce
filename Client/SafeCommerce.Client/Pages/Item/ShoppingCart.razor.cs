using SafeCommerce.ClientDTO.Item;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Pages.Item;

public partial class ShoppingCart
{
    [Inject] private ShoppingCartService ShoppingCartService { get; set; } = null!;

    private async Task 
    IncreaseQuantity
    (
        ClientDto_CartItem item
    )
    {
        await ShoppingCartService.IncreaseQuantityAsync(item);
    }

    private async Task
    DecreaseQuantity
    (
        ClientDto_CartItem item
    )
    {
        await ShoppingCartService.DecreaseQuantityAsync(item);
    }

    private async Task
    RemoveItem
    (
        ClientDto_CartItem item
    )
    {
        await ShoppingCartService.RemoveItemAsync(item);
    }

    private async void Checkout()
    {
        await ShoppingCartService.CheckOut();
    }
}