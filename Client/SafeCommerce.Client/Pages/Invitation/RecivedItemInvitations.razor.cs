using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Invitation;

public partial class RecivedItemInvitations
{
    [Inject] public ISnackbar _snackbar { get; set; } = null!;
    [Inject] ILocalStorageService LocalStorage { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] IClientService_Invitation InvitationService { get; set; } = null!;
    [Inject] IAuthenticationService AuthenticationService { get; set; } = null!;

    private List<ClientDto_RecivedItemInvitations> RecivedInvitationsList { get; set; } = [];

    private bool _processingRejectInvitation = false;
    private bool _processingAcceptInvitation = false;

    protected override async Task OnInitializedAsync()
    {
        var role = await LocalStorage.GetItemAsStringAsync("Role");

        if (role == null || role != Role.User.ToString())
            await LogOutHelper.LogOut(NavigationManager, LocalStorage, AuthenticationService);

        var getSentInvitations = await InvitationService.GetItemsInvitations();

        if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
            RecivedInvitationsList = getSentInvitations.Value;
    }

    private async Task
    RejectInvitation
    (
        ClientDto_RecivedItemInvitations recivedInvitation
    )
    {
        _processingRejectInvitation = true;

        var rejectInvitationResult = await InvitationService.RejectItemInvitation(new ClientDto_InvitationItemRequestActions
        {
            ItemId = recivedInvitation.ItemId,
            InvitingUserId = recivedInvitation.InvitingUserId,
            InvitationId = recivedInvitation.InvitationId,
        });

        if (rejectInvitationResult.Succsess)
            RecivedInvitationsList.Remove(recivedInvitation);

        _processingRejectInvitation = false;
    }

    private async Task
    AcceptInvitation
    (
        ClientDto_RecivedItemInvitations recivedInvitation
    )
    {
        _processingAcceptInvitation = true;

        var acceptInvitationResult = await InvitationService.AcceptItemInvitation(new ClientDto_InvitationItemRequestActions
        {
            ItemId = recivedInvitation.ItemId,
            InvitingUserId = recivedInvitation.InvitingUserId,
            InvitationId = recivedInvitation.InvitationId,
        });

        if (acceptInvitationResult.Succsess)
            RecivedInvitationsList.Find(x => x.InvitationId == recivedInvitation.InvitationId).InvitationStatus = InvitationStatus.Accepted;

        switch (acceptInvitationResult.StatusCode)
        {
            case System.Net.HttpStatusCode.OK:
                _snackbar.Add(acceptInvitationResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            case System.Net.HttpStatusCode.BadRequest:
                _snackbar.Add(acceptInvitationResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            case System.Net.HttpStatusCode.InternalServerError:
                _snackbar.Add(acceptInvitationResult.Message, Severity.Error, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
                break;
            default:
                break;
        }

        _processingAcceptInvitation = false;

        await InvokeAsync(StateHasChanged);
    }
}