using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Shared.Components.Item;

public partial class ItemsDataTable
{
    [Parameter] public IEnumerable<ClientDto_Item> listItems { get; set; } = null!;

    private bool Dense = false;
    private bool Hover = true;
    private bool Striped = false;
    private bool Bordered = false;
    private string SearchString = "";
    private ClientDto_Item? SelectedItem = null;

    private bool FilterFunc(ClientDto_Item element) => FilterFunc(element, SearchString);

    private static bool FilterFunc(ClientDto_Item element, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (element.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.OwnerName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if (element.Price.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        if ($"is approved={element.IsApproved}".Contains(searchString))
            return true;
        return false;
    }
}