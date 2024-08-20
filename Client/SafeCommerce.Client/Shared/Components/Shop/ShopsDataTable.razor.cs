using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class ShopsDataTable
{
    [Parameter] public List<ClientDto_Shop> ListShops { get; set; } = null!;

    private bool Dense = false;
    private bool Hover = true;
    private bool Striped = false;
    private bool Bordered = false;
    private string SearchString = "";
    private ClientDto_Shop? SelectedShop = null;

    private bool FilterFunc1(ClientDto_Shop element) => FilterFunc(element, SearchString);

    private static bool FilterFunc(ClientDto_Shop element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OwnerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}