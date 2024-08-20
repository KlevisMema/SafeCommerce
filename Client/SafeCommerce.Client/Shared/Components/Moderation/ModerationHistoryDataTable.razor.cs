using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Moderation;

namespace SafeCommerce.Client.Shared.Components.Moderation;

public partial class ModerationHistoryDataTable
{
    [Parameter] public ClientDto_SplittedModerationHistory ModerationHistory { get; set; }

    private bool Dense = false;
    private bool Hover = true;
    private bool Striped = false;
    private bool Bordered = false;
    private string SearchString = "";
    private ClientDto_Shop? SelectedShop = null;

    private bool FilterFunc1(ClientDto_ModerationHistory element) => FilterFunc(element, SearchString);

    private static bool FilterFunc(ClientDto_ModerationHistory element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.ShopName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.ItemName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}