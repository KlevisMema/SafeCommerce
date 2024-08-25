using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.Client.Shared.Forms.Shop;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.Client.Shared.Forms.Item;
using SafeCommerce.Client.Shared.Components.Shop;

namespace SafeCommerce.Client.Shared.Components.Item;

public partial class UserRoleItems
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_Item> ListItems { get; set; } = [];
    private ClientDto_GroupedItems GroupedItems { get; set; } = new();
    private List<ClientDto_ItemMembers> CurrentItemMembers { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var getListItems = await this.ItemService.GetUserItems();
        string? userId = await LocalStorageService.GetItemAsStringAsync("Id");
        string? Role = await LocalStorageService.GetItemAsStringAsync("Role");

        if (Role != SafeCommerce.ClientDTO.Enums.Role.User.ToString())
            NavigationManager.NavigateTo("Not-Found");

        if (string.IsNullOrEmpty(userId))
            await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);

        if (getListItems != null && getListItems.Succsess && getListItems.Value is not null)
        {
            foreach (var item in getListItems.Value)
            {
                bool isValidCrypoKey = await this.JsRuntime.InvokeAsync<bool>("verifyPublicKey", item.OwnerPublicKey, item.OwnerSigningPublicKey, item.OwnerSignature);

                if (isValidCrypoKey)
                {
                    if (!item.MakePublic)
                    {
                        ClientDto_Item decryptedItem = new();

                        if (userId == item.OwnerId.ToString())
                        {
                            decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_Item>("decryptMyItemData", item, userId);
                            decryptedItem.Price = decimal.Parse(decryptedItem.PriceDecrypted);
                        }
                        else
                        {
                            decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_Item>("decryptItemData", item, userId);
                            decryptedItem.Price = decimal.Parse(decryptedItem.PriceDecrypted);
                        }

                        this.ListItems.Add(decryptedItem);
                        continue;
                    }

                    this.ListItems.Add(item);
                }
            }
        }

        GroupedItems = GroupShops(ListItems, userId!);
    }

    public static ClientDto_GroupedItems
    GroupShops
    (
        IEnumerable<ClientDto_Item> items,
        string currentUserId
    )
    {
        var groupedItems = new ClientDto_GroupedItems();

        foreach (var item in items)
        {
            if (item.OwnerId.ToString() == currentUserId)
            {
                if (item.MakePublic && item.IsPublic && item.IsApproved && item.ShopId == Guid.Empty)
                    groupedItems.MyPublicItemsSharedWithEveryone.Add(item);
                else if (!item.MakePublic && !item.IsPublic && !item.IsApproved && item.ShopId != Guid.Empty)
                    groupedItems.MyPrivateItemsSharedWithPrivateShop.Add(item);
                else if (item.MakePublic && item.IsPublic && item.IsApproved && item.ShopId != Guid.Empty)
                    groupedItems.MyPublicItemsSharedWithPublicShop.Add(item);
                else
                    groupedItems.MyPrivateItemsSharedWithSpecificUsers.Add(item);
            }
            else
                groupedItems.PrivateItemsSharedWithMe.Add(item);
        }

        return groupedItems;
    }

    private async Task
    EditItemOpenDialog
    (
        ClientDto_Item item
    )
    {
        var parameters = new DialogParameters
        {
            { "ItemToBeEdited", item },
            { "OnItemUpdated", EventCallback.Factory.Create(this, (ClientDto_Item updatedItem) => OnItemUpdated(updatedItem, ref item)) }
        };

        var dialog = await DialogService.ShowAsync<EditItemForm>("Edit Item", parameters, DialogHelper.SimpleDialogOptions());
        var result = await dialog.Result;
    }

    private void
    OnItemUpdated
    (
        ClientDto_Item updatedIem,
        ref ClientDto_Item oldItemData
    )
    {
        oldItemData.Name = updatedIem.Name;
        oldItemData.Price = updatedIem.Price;
        oldItemData.Picture = updatedIem.Picture;
        oldItemData.Description = updatedIem.Description;
        InvokeAsync(StateHasChanged);
    }

    private async Task
    DeleteItemOpenDialog
    (
        ClientDto_Item item,
        bool isPrivateItem
    )
    {
        var parameters = new DialogParameters
        {
            { "ItemToBeDeleted", item },
            { "PrivateItem", isPrivateItem },
            { "OnItemDeleted", EventCallback.Factory.Create(this, () => OnItemDeleted(item)) }
        };

        var dialog = await DialogService.ShowAsync<DeleteItem>("Are you sure you want to delete this item?", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private async Task
    OpenInvitationDialog
    (
        ClientDto_Item item,
        bool isPrivateItem
    )
    {
    }

    private async Task
    OnItemDeleted
    (
        ClientDto_Item item
    )
    {
        GroupedItems.MyPrivateItemsSharedWithPrivateShop.Remove(item);
        GroupedItems.MyPrivateItemsSharedWithSpecificUsers.Remove(item);
        GroupedItems.MyPublicItemsSharedWithPublicShop.Remove(item);
        GroupedItems.MyPublicItemsSharedWithEveryone.Remove(item);

        _snackbar.Add("Item deleted succsessfuly", Severity.Success, config => { config.CloseAfterNavigation = true; });
    }

    private async Task
    OpenMembersDialog
    (
        Guid itemId,
        bool imOwner
    )
    {
        var itemMembers = await ItemService.GetMembersOfTheItem(itemId);

        var parameters = new DialogParameters<List<ClientDto_ItemMembers>> {
            { "ItemService", ItemService } ,
            { "_snackbar", _snackbar },
            { "ImOwner", imOwner },
            { "ItemId", itemId },
            { "CurrentItemMembersList", itemMembers.Value.ToList() },
            { "OnMemberRemoved", EventCallback.Factory.Create(this, () => OnMemberRemoved()) }
        };

        var dialog = await DialogService.ShowAsync<CurrentItemMembers>("Item Members", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private async Task
    OnMemberRemoved()
    {
        ListItems = [];
        CurrentItemMembers = [];
        GroupedItems = new();
        await OnInitializedAsync();
    }
}