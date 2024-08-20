using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.Client.Shared.Forms.Shop;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class UserRoleShops
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private List<ClientDto_Shop> ListShops { get; set; } = [];
    private List<ClientDto_ShopShare> CurrentShopMembers { get; set; } = [];
    private ClientDto_GroupedShops GroupedShops { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var getListShops = await this.ShopService.GetUserShops();
        string? userId = await LocalStorageService.GetItemAsStringAsync("Id");
        string? Role = await LocalStorageService.GetItemAsStringAsync("Role");

        if (Role != SafeCommerce.ClientDTO.Enums.Role.User.ToString())
            NavigationManager.NavigateTo("Not-Found");

        if (getListShops != null && getListShops.Succsess && getListShops.Value is not null)
        {
            foreach (var shop in getListShops.Value)
            {
                if (!shop.MakePublic)
                {
                    ClientDto_Shop decryptedShop = new();
                    if (userId == shop.OwnerId.ToString())
                        decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_Shop>("decryptMyShopData", shop, userId);
                    else
                        decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_Shop>("decryptShopData", shop, userId);

                    decryptedShop.Items = shop.Items;
                    decryptedShop.ShopShares = shop.ShopShares;
                    decryptedShop.CreatedAt = shop.CreatedAt;
                    decryptedShop.ModifiedAt = shop.ModifiedAt;
                    decryptedShop.MemberCount = shop.MemberCount;
                    decryptedShop.ItemCount = shop.ItemCount;
                    this.ListShops.Add(decryptedShop);
                    continue;
                }

                this.ListShops.Add(shop);
            }
        }

        GroupedShops = GroupShops(ListShops, userId);
    }

    public static ClientDto_GroupedShops
    GroupShops
    (
        IEnumerable<ClientDto_Shop> shops,
        string currentUserId
    )
    {
        var groupedShops = new ClientDto_GroupedShops();

        foreach (var shop in shops)
        {
            if (shop.OwnerId.ToString() == currentUserId)
            {
                if (shop.MakePublic)
                    groupedShops.MyPublicShops.Add(shop);
                else
                    groupedShops.MyPrivateShops.Add(shop);
            }
            else
                groupedShops.JoinedShops.Add(shop);
        }

        return groupedShops;
    }

    private void ViewItems(Guid shopId)
    {
    }

    private async Task
    EditShopOpenDialog
    (
        ClientDto_Shop clientDto_Shop
    )
    {
        var parameters = new DialogParameters
            {
                { "ShopToBeEdited", clientDto_Shop },
                { "OnShopUpdated", EventCallback.Factory.Create(this, (ClientDto_Shop updatedShop) => OnShopUpdated(updatedShop, ref clientDto_Shop)) }
            };

        var dialog = await DialogService.ShowAsync<EditShopForm>("Edit Group", parameters, DialogHelper.SimpleDialogOptions());
        var result = await dialog.Result;
    }

    private void
    OnShopUpdated
    (
        ClientDto_Shop updatedShop,
        ref ClientDto_Shop oldShopData
    )
    {
        oldShopData.Name = updatedShop.Name;
        oldShopData.Description = updatedShop.Description;
        InvokeAsync(StateHasChanged);
    }

    private async Task
    DeleteShopOpenDialog
    (
        ClientDto_Shop clientDto_Shop,
        bool isPrivateShop
    )
    {
        var parameters = new DialogParameters
            {
                { "ShopToBeDeleted", clientDto_Shop },
                { "PrivateShop", isPrivateShop },
                { "OnShopDeleted", EventCallback.Factory.Create(this, () => OnDeletedShop(clientDto_Shop, isPrivateShop)) }
            };

        var dialog = await DialogService.ShowAsync<DeleteShop>("Are you sure you want to delete this group?", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private void
    OnDeletedShop
    (
        ClientDto_Shop clientDto_Shop,
        bool isPrivateShop
    )
    {
        if (isPrivateShop)
            GroupedShops.MyPrivateShops.Remove(clientDto_Shop);
        else
            GroupedShops.MyPublicShops.Remove(clientDto_Shop);

        _snackbar.Add("Goup deleted succsessfuly", Severity.Success, config => { config.CloseAfterNavigation = true; });
    }

    private async Task
    OpenInvitationDialog
    (
        ClientDto_Shop shop,
        bool isPrivateShop
    )
    {
        var parameters = new DialogParameters
            {
                { "Shop", shop },
                { "IsPrivateShop", isPrivateShop },
            };

        var dialog = await DialogService.ShowAsync<InviteUserToShop>("Invite user to shop", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private async Task
    OpenMembersDialog
    (
        Guid shopId,
        bool imOwner
    )
    {
        var selectedShop = GroupedShops.MyPrivateShops.FirstOrDefault(s => s.ShopId == shopId)
                           ?? GroupedShops.MyPublicShops.FirstOrDefault(s => s.ShopId == shopId)
                           ?? GroupedShops.JoinedShops.FirstOrDefault(s => s.ShopId == shopId);

        if (selectedShop != null && selectedShop.ShopShares is not null)
        {
            CurrentShopMembers = selectedShop.ShopShares.ToList() ?? [];
            CurrentShopMembers.Insert(0, new ClientDto_ShopShare
            {
                UserName = selectedShop.OwnerName,
            });
        }

        else
            CurrentShopMembers = [];

        var parameters = new DialogParameters<List<ClientDto_ShopShare>> {
            { "CurrentShopMembersList", CurrentShopMembers },
            { "ShopService", ShopService } ,
            { "_snackbar", _snackbar },
            { "ImOwner", imOwner },
            { "OnMemberRemoved", EventCallback.Factory.Create(this, () => OnMemberRemoved()) }
        };

        var dialog = await DialogService.ShowAsync<CurrentShopMembers>("Group Members", parameters, DialogHelper.SimpleDialogOptions());
        await dialog.Result;
    }

    private async Task
    OnMemberRemoved()
    {
        ListShops = [];
        CurrentShopMembers = [];
        GroupedShops = new();
        await OnInitializedAsync();
    }
}