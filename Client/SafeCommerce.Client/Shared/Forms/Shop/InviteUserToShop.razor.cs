using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.Client.Pages.Item;

namespace SafeCommerce.Client.Shared.Forms.Shop;

public partial class InviteUserToShop
{
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IClientService_Invitation ServiceInvitation { get; set; } = null!;
    [Inject] private IClientService_UserManagment UserManagmentService { get; set; } = null!;

    [Parameter] public bool IsPrivateShop { get; set; }
    [Parameter] public ClientDto_Shop? Shop { get; set; }
    [Parameter] public InvitationReason InvitationReason { get; set; }
    [Parameter] public EventCallback<ClientDto_UserSearched> OnUserSelected { get; set; }

    private bool _processing = false;
    private ClientDto_UserSearched? SelectedUser { get; set; }
    private ClientDto_SendInvitationRequest SendInvitationRequest { get; set; } = new();

    private async Task
    ValidateForm()
    {
        if (SelectedUser is not null)
        {
            if (String.IsNullOrEmpty(SelectedUser.PublicKey) || String.IsNullOrEmpty(SelectedUser.Signature) || String.IsNullOrEmpty(SelectedUser.SigningPublicKey))
            {
                ShowValidationsMessages(["User can be invited!"]);
            }
            if (InvitationReason == InvitationReason.ShopInvitation)
                await SubmitInviteUserToGroupForm();
            else
            {
                await OnUserSelected.InvokeAsync(SelectedUser);
                MudDialog.Close();
            }
        }
        else
            ShowValidationsMessages(["Please select a user!"]);
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
    SubmitInviteUserToGroupForm()
    {
        _processing = true;
        await Task.Delay(1000);

        SendInvitationRequest.ShopId = Shop.ShopId;
        SendInvitationRequest.InvitedUserId = Guid.Parse(SelectedUser.UserId);

        if (IsPrivateShop)
        {
            var senderUserId = await LocalStorageService.GetItemAsStringAsync("Id");

            var encryptedKeysForUser = await JsRuntime.InvokeAsync<ClientDto_SendInvitationRequest>("inviteUserToShop", SelectedUser.PublicKey, senderUserId, Shop.ShopId);

            SendInvitationRequest.EncryptedKey = encryptedKeysForUser.EncryptedKey;
            SendInvitationRequest.EncryptedKeyNonce = encryptedKeysForUser.EncryptedKeyNonce;

            if (Shop.Items is not null && Shop.Items.Count != 0)
            {
                SendInvitationRequest.Items = [];
                foreach (var item in Shop.Items)
                {
                    string? decryptedItemSymmetricKey = await JsRuntime.InvokeAsync<string>("decryptMyItemSymmericKey", senderUserId, item.ItemId);

                    if (!String.IsNullOrEmpty(decryptedItemSymmetricKey))
                    {
                        ClientDto_ShareItem? shareItemWithUser = await JsRuntime.InvokeAsync<ClientDto_ShareItem>("shareItemToUser", SelectedUser.PublicKey, decryptedItemSymmetricKey, senderUserId);
                        
                        SendInvitationRequest.Items.Add(new ClientDto_Item
                        {
                             ItemId = item.ItemId,
                             EncryptedKey = shareItemWithUser?.EncryptedKey,
                             EncryptedKeyNonce = shareItemWithUser?.EncryptedKeyNonce,
                        });
                    }
                }
            }
        }

        var SendInvitationResult = await ServiceInvitation.SendInvitation(SendInvitationRequest);

        if (!SendInvitationResult.Succsess)
        {
            Snackbar.Add(SendInvitationResult.Message, Severity.Warning,
                config => { config.CloseAfterNavigation = true; });

            if (SendInvitationResult.Errors is not null)
                ShowValidationsMessages(SendInvitationResult.Errors);
        }
        else
        {
            Snackbar.Add(SendInvitationResult.Message, Severity.Success,
                config => { config.CloseAfterNavigation = true; });

        }

        SendInvitationRequest = new();
        _processing = false;
        MudDialog.Close();
    }

    private async Task<IEnumerable<ClientDto_UserSearched>>
    Search
    (
       string value,
       CancellationToken token
    )
    {
        var result = await UserManagmentService.SearchUserByUserName(value ?? string.Empty, token);

        if (!result.Succsess && result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
        {
            Snackbar.Add
            (
                result.Message,
                result.StatusCode == System.Net.HttpStatusCode.InternalServerError ? Severity.Error : Severity.Warning,
                config =>
                {
                    config.CloseAfterNavigation = true;
                    config.VisibleStateDuration = 3000;
                }
            );
        }

        return result.Value ?? Enumerable.Empty<ClientDto_UserSearched>();
    }
}