using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.Client.Internal.Helpers;

namespace SafeCommerce.Client.Pages.Invitation;

public partial class SentItemInvitations
{
    [Inject] IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] public ISnackbar _snackbar { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] IClientService_Invitation InvitationService { get; set; } = null!;
    [Inject] IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_SentItemInvitations> SentInvitationsList { get; set; } = [];

    private bool _processingDeleteInvitation = false;

    protected override async Task OnInitializedAsync()
    {
        var role = await LocalStorageService.GetItemAsStringAsync("Role");

        if (role == null || role != Role.User.ToString())
            await LogOutHelper.LogOut(NavigationManager, LocalStorageService, AuthenticationService);

        var getSentInvitations = await InvitationService.GetSentItemInvitations();

        if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
        {
            string? userId = await LocalStorageService.GetItemAsStringAsync("Id");
            foreach (var invitationItem in getSentInvitations.Value)
            {
                if (!invitationItem.IsPublic)
                {
                    var decryptedItem = await this.JsRuntime.InvokeAsync<ClientDto_SentItemInvitations>("decryptSentItemInvitation", invitationItem, userId);
                    this.SentInvitationsList.Add(decryptedItem);
                    continue;
                }

                this.SentInvitationsList.Add(invitationItem);
            }
        }
    }

    private async void
    DeleteInvitation
    (
        ClientDto_SentItemInvitations SentInvitation
    )
    {
        _processingDeleteInvitation = true;

        var deleteInvitationResult = await InvitationService.DeleteItemInvitation(new ClientDto_InvitationItemRequestActions
        {
            ItemId = SentInvitation.ItemId,
            InvitedUserId = SentInvitation.InvitedUserId,
            InvitationId = SentInvitation.InvitationId,
        });

        if (deleteInvitationResult.Succsess)
            SentInvitationsList.Remove(SentInvitationsList.Find(x => x.InvitationId == SentInvitation.InvitationId));

        switch (deleteInvitationResult.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                _snackbar.Add(deleteInvitationResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            case System.Net.HttpStatusCode.BadRequest:
                _snackbar.Add(deleteInvitationResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            case System.Net.HttpStatusCode.InternalServerError:
                _snackbar.Add(deleteInvitationResult.Message, Severity.Error, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            default:
                break;
        }

        _processingDeleteInvitation = false;

        StateHasChanged();
    }
}