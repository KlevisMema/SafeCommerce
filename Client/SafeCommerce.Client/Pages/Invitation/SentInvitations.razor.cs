using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Invitation;

public partial class SentInvitations
{
    [Inject] IClientService_Invitation InvitationService { get; set; } = null!;
    [Inject] ILocalStorageService LocalStorageService { get; set; } = null!;
    [Inject] IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] public ISnackbar _snackbar { get; set; } = null!;
    private List<ClientDto_SentInvitations> SentInvitationsList { get; set; } = [];
    private bool _processingDeleteInvitation = false;

    protected override async Task OnInitializedAsync()
    {
        var getSentInvitations = await InvitationService.GetSentShopInvitations();

        if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
        {
            string? userId = await LocalStorageService.GetItemAsStringAsync("Id");
            foreach (var invitationShop in getSentInvitations.Value)
            {
                if (!invitationShop.IsPublic)
                {
                    var decryptedShop = await this.JsRuntime.InvokeAsync<ClientDto_SentInvitations>("decryptSentInvitation", invitationShop, userId);
                    this.SentInvitationsList.Add(decryptedShop);
                    continue;
                }

                this.SentInvitationsList.Add(invitationShop);
            }
        }
    }

    private async void
    DeleteInvitation
    (
        ClientDto_SentInvitations SentInvitation
    )
    {
        _processingDeleteInvitation = true;

        var deleteInvitationResult = await InvitationService.DeleteInvitation(new ClientDto_InvitationRequestActions
        {
            ShopId = SentInvitation.ShopId,
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