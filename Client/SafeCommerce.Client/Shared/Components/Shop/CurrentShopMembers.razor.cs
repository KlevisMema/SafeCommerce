using MudBlazor;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class CurrentShopMembers
{
    [Parameter] public ISnackbar _snackbar { get; set; } = null!;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public IClientService_Shop ShopService { get; set; }
    [Parameter] public List<ClientDto_ShopShare> CurrentShopMembersList { get; set; }
    [Parameter] public EventCallback OnMemberRemoved { get; set; }

    private bool _processing = false;
    [Parameter] public bool ImOwner {get; set; }


    private async Task
    RemoveUserFromShop
    (
        ClientDto_ShopShare clientDto_ShopShare
    )
    {
        _processing = true;
        await Task.Delay(1000);

        var removedUserResult = await ShopService.RemoveUserFromShop(new ClientDto_RemoveUserFromShop
        {
             ShopId = clientDto_ShopShare.ShopId,
              UserId = Guid.Parse(clientDto_ShopShare.UserId),
        });

        if (removedUserResult.Succsess)
        {
            _snackbar.Add(removedUserResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
            CurrentShopMembersList.Remove(clientDto_ShopShare);

            await OnMemberRemoved.InvokeAsync();
        }

        _processing = false;

        await InvokeAsync(StateHasChanged);
    }
}