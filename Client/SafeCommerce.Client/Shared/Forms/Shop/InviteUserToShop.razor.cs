using MudBlazor;
using Microsoft.JSInterop;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientDTO.AccountManagment;
using SafeCommerce.ClientServices.Interfaces;
using Blazored.LocalStorage;

namespace SafeCommerce.Client.Shared.Forms.Shop;

public partial class InviteUserToShop
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] private IClientService_Invitation ServiceInvitation { get; set; } = null!;
    [Inject] private IClientService_UserManagment UserManagmentService { get; set; } = null!;

    [Parameter] public ClientDto_Shop? Shop { get; set; }
    [Parameter] public bool IsPrivateShop { get; set; }

    private bool _processing = false;
    private ClientDto_UserSearched? SelectedUser { get; set; }
    private ClientDto_SendInvitationRequest SendInvitationRequest { get; set; } = new();

    private async Task
    ValidateForm()
    {
        if (SelectedUser is not null)
            await SubmitInviteUserToGroupForm();
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