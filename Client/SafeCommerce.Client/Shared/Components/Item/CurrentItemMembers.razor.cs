using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Item;

public partial class CurrentItemMembers
{
    [Parameter] public ISnackbar _snackbar { get; set; } = null!;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public IClientService_Item ItemService { get; set; }
    [Parameter] public List<ClientDto_ItemMembers> CurrentItemMembersList { get; set; }
    [Parameter] public EventCallback OnMemberRemoved { get; set; }
    [Parameter] public Guid ItemId { get; set; }

    private bool _processing = false;
    [Parameter] public bool ImOwner { get; set; }


    private async Task
    RemoveUserFromItem
    (
        ClientDto_ItemMembers clientDto_ShareItem
    )
    {
        _processing = true;
        await Task.Delay(1000);

        var removedUserResult = await ItemService.RemoveUserFromItem(new ClientDto_RemoveUserFromItem
        {
            ItemId = ItemId,
            UserId = Guid.Parse(clientDto_ShareItem.UserId),
        });

        if (removedUserResult.Succsess)
        {
            _snackbar.Add(removedUserResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
            CurrentItemMembersList.Remove(clientDto_ShareItem);

            await OnMemberRemoved.InvokeAsync();
        }

        _processing = false;

        await InvokeAsync(StateHasChanged);
    }
}