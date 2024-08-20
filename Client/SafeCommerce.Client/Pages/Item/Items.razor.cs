using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Item;

public partial class Items
{
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    private IEnumerable<ClientDto_Item> ListItems { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getListItems = await ItemService.GetUserItems();

        if (getListItems != null && getListItems.Succsess && getListItems.Value is not null)
            this.ListItems = getListItems.Value;
    }
}