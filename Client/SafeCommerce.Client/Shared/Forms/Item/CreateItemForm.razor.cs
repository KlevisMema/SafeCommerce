using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.Client.Shared.Forms.Shop;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Shared.Forms.Item;

public partial class CreateItemForm
{

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IAuthenticationService AuthenticationService { get; set; } = null!;

    private string? imagePreview;
    private bool _processing = false;
    private EditForm? CreateItemFormContext;
    private IBrowserFile? ImageFile { get; set; }
    private List<ClientDto_Shop> ListShops { get; set; } = new();
    private ClientDto_UserSearched? SelectedUser { get; set; }
    private ClientDto_CreateItem CreateItem { get; set; } = new();
    private string visibilityClassForShop { get; set; } = "block";
    private string SelectOptionIcon { get; set; } = Icons.Material.Filled.Shop;

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
                ClientDto_Shop decryptedShop = new();

                if (userId == shop.OwnerId.ToString())
                {
                    if (!shop.IsPublic && !shop.MakePublic)
                    {
                        decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_Shop>("decryptMyShopData", shop, userId);
                        decryptedShop.Items = shop.Items;
                        decryptedShop.ShopShares = shop.ShopShares;
                        decryptedShop.CreatedAt = shop.CreatedAt;
                        decryptedShop.ModifiedAt = shop.ModifiedAt;
                        decryptedShop.MemberCount = shop.MemberCount;
                        decryptedShop.ItemCount = shop.ItemCount;
                        this.ListShops.Add(decryptedShop);
                    }
                    else
                    {
                        this.ListShops.Add(shop);
                    }
                }
            }
        }
    }

    private async Task
    UploadFile
    (
        IBrowserFile file
    )
    {
        if (file is not null)
        {
            ImageFile = file;
            CreateItem.Picture = await ConvertToBase64StringAsync(file);
            imagePreview = $"data:{file.ContentType};base64," + await ConvertToBase64StringAsync(file);
        }

        StateHasChanged();
    }

    private async Task
    ValidateForm()
    {
        var validationsPassed = CreateItemFormContext!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(CreateItemFormContext.EditContext.GetValidationMessages());
        else
            await SubmitCreateShopForm();
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages
    )
    {
        foreach (var validationMessage in validationMessages)
        {
            Snackbar.Add(validationMessage, Severity.Warning, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });
        }
    }

    private async Task
    SubmitCreateShopForm()
    {
        _processing = true;
        await Task.Delay(1000);

        switch (CreateItem.ItemShareOption)
        {
            case ItemShareOption.Shop:
                await ShareItemToShop();
                break;
            case ItemShareOption.ToUser:
                await ShareItemToUser();
                break;
            case ItemShareOption.Everybody:
                await ShareItemToEveryone();
                break;
            default:
                break;
        }

        _processing = false;
    }

    private async Task
    ShareItemToShop()
    {
        string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

        if (userId == null)
        {
            Snackbar.Add("Something wen't wrong, please log in again!", Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);
            return;
        }

        var selectedShop = ListShops.FirstOrDefault(x => x.ShopId == CreateItem.ShopId);

        if (selectedShop == null) {
            Snackbar.Add("Please select a shop again", Severity.Warning, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            return;
        }

        if (!selectedShop.MakePublic && !selectedShop.IsPublic)
        {
            await PrivateShop(userId);
        }
        else
        {
            await PublicShop();
        }
    }

    private async Task
    PrivateShop
    (
        string userId
    )
    {
        List<ClientDto_ShopMembers> verifiedMembers = await GetVerifiedMembers();

        ClientDto_CreateItemWithAllKeys? myEncryptedItem = await JsRuntime.InvokeAsync<ClientDto_CreateItemWithAllKeys>("encryptItemData", CreateItem, userId);
        
        if (myEncryptedItem == null)
        {

            Snackbar.Add("Something wen't wrong, item could not be encrypted try again", Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            return;
        }

        CreateItem.ShopId = myEncryptedItem.ShopId;

        CreateItem.Picture = myEncryptedItem.Picture;
        CreateItem.Name = myEncryptedItem.Name;
        CreateItem.Price = 0;
        CreateItem.Description = myEncryptedItem.Description;
        CreateItem.EncryptedPrice = myEncryptedItem.EncryptedPrice;

        CreateItem.DataNonce = myEncryptedItem.DataNonce;
        CreateItem.EncryptedKey = myEncryptedItem.EncryptedKey;
        CreateItem.SignatureOfKey = myEncryptedItem.SignatureOfKey;
        CreateItem.SigningPublicKey = myEncryptedItem.SigningPublicKey;
        CreateItem.EncryptedKeyNonce = myEncryptedItem.EncryptedKeyNonce;

        CreateItem.ItemShareOption = myEncryptedItem.ItemShareOption;

        CreateItem.ShareItemToPrivateShop = new List<ClientDto_ShareItem>();

        foreach (var member in verifiedMembers)
        {
            ClientDto_ShareItem? shareItemWithUser = await JsRuntime.InvokeAsync<ClientDto_ShareItem>("shareItemToUser", member.PublicKey, myEncryptedItem.NonEncryptedKey, userId);
            shareItemWithUser.UserId = member.UserId;
            shareItemWithUser.ShopId = CreateItem.ShopId;
            CreateItem.ShareItemToPrivateShop.Add(shareItemWithUser);
        }

        CreateItem.DTO_ShareItem = null;

        await SubmitForm();
    }

    private async Task<List<ClientDto_ShopMembers>> 
    GetVerifiedMembers()
    {
        var shopMembers = await ShopService.GetMembersOfTheShop(CreateItem.ShopId);

        List<ClientDto_ShopMembers> verifiedMembers = new();

        if (shopMembers.Succsess && shopMembers.Value is not null)
        {
            foreach (var member in shopMembers.Value)
            {
                bool verified = await JsRuntime.InvokeAsync<bool>("verifyPublicKey", member.PublicKey, member.SigningPublicKey, member.Signature);

                if (verified)
                    verifiedMembers.Add(member);
            }
        }

        return verifiedMembers;
    }

    private async Task
    PublicShop()
    {
        CreateItem.ShareItemToPrivateShop = null;
        CreateItem.DTO_ShareItem = null;
        CreateItem.EncryptedPrice = null;

        await SubmitForm();
    }

    private async Task
    ShareItemToEveryone()
    {
        CreateItem.DTO_ShareItem = null;
        CreateItem.ShareItemToPrivateShop = null;
        await SubmitForm();
    }

    private async Task
    ShareItemToUser()
    {
        if (SelectedUser is null)
        {
            Snackbar.Add("Please select a user again", Severity.Warning, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            return;
        }

        bool isValidUser = await JsRuntime.InvokeAsync<bool>("verifyPublicKey", SelectedUser.PublicKey, SelectedUser.SigningPublicKey, SelectedUser.Signature);

        if (!isValidUser)
        {
            Snackbar.Add("User is not valid ,please select a user again", Severity.Warning, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            return;
        }

        string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

        if (userId == null)
        {
            Snackbar.Add("Something wen't wrong, please log in again!", Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);
            return;
        }

        ClientDto_CreateItemWithAllKeys? myEncryptedItem = await JsRuntime.InvokeAsync<ClientDto_CreateItemWithAllKeys>("encryptItemData", CreateItem, userId);

        if (myEncryptedItem == null)
        {

            Snackbar.Add("Something wen't wrong, item could not be encrypted try again", Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            return;
        }

        ClientDto_ShareItem? shareItemWithUser = await JsRuntime.InvokeAsync<ClientDto_ShareItem>("shareItemToUser", SelectedUser.PublicKey, myEncryptedItem.NonEncryptedKey, userId);
        shareItemWithUser.UserId = SelectedUser.UserId;

        CreateItem.ShopId = myEncryptedItem.ShopId;

        CreateItem.Price = null;
        CreateItem.Picture = myEncryptedItem.Picture;
        CreateItem.Name = myEncryptedItem.Name;
        CreateItem.Description = myEncryptedItem.Description;
        CreateItem.EncryptedPrice = myEncryptedItem.EncryptedPrice;

        CreateItem.DataNonce = myEncryptedItem.DataNonce;
        CreateItem.EncryptedKey = myEncryptedItem.EncryptedKey;
        CreateItem.SignatureOfKey = myEncryptedItem.SignatureOfKey;
        CreateItem.SigningPublicKey = myEncryptedItem.SigningPublicKey;
        CreateItem.EncryptedKeyNonce = myEncryptedItem.EncryptedKeyNonce;

        CreateItem.ItemShareOption = myEncryptedItem.ItemShareOption;

        CreateItem.DTO_ShareItem = shareItemWithUser;

        CreateItem.ShareItemToPrivateShop = null;

        await SubmitForm();
    }

    private async Task
    SubmitForm()
    {
        var result = await ItemService.CreateItem(CreateItem);

        if (!result.Succsess)
        {
            Snackbar.Add(result.Message, Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Success, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });

            CreateItem = new();
            ImageFile = null;
            imagePreview = string.Empty;
            SelectedUser = null;
            visibilityClassForShop = "block";
        }

        await InvokeAsync(StateHasChanged);
    }

    private static async Task<byte[]>
    ConvertToByteArrayAsync
    (
        IBrowserFile file
    )
    {
        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    private async Task
    ValueSelected()
    {
        if (CreateItem.ItemShareOption == ItemShareOption.ToUser)
        {
            SelectOptionIcon = Icons.Material.Filled.Person;
            visibilityClassForShop = "none";
            CreateItem.ShopId = Guid.Empty;

            var parameters = new DialogParameters
            {
                { "InvitationReason", InvitationReason.UserInvitation },
                { "OnUserSelected", EventCallback.Factory.Create(this, (ClientDto_UserSearched? SelectedUser) => OnUserSelected(SelectedUser)) }
            };

            var dialog = await DialogService.ShowAsync<InviteUserToShop>("Share item to user", parameters, DialogHelper.SimpleDialogOptions());
            await dialog.Result;
        }
        else if (CreateItem.ItemShareOption == ItemShareOption.Everybody)
        {
            visibilityClassForShop = "none";
            SelectedUser = null;
            CreateItem.ShopId = Guid.Empty;
            SelectOptionIcon = Icons.Material.Filled.Groups;
        }
        else
        {
            visibilityClassForShop = "block";
            SelectedUser = null;
            SelectOptionIcon = Icons.Material.Filled.Shop2;
        }

        StateHasChanged();
    }

    private void
    OnUserSelected
    (
        ClientDto_UserSearched? SelectedUser
    )
    {
        if (SelectedUser is not null)
            this.SelectedUser = SelectedUser;
    }

    private static async Task<string>
    ConvertToBase64StringAsync
    (
        IBrowserFile file
    )
    {
        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        return Convert.ToBase64String(fileBytes);
    }
}