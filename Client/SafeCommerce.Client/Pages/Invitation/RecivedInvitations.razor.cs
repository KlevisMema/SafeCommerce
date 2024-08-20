using MudBlazor;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Invitation;

public partial class RecivedInvitations
{
    [Inject] IClientService_Invitation InvitationService { get; set; } = null!;
    [Inject] public ISnackbar _snackbar { get; set; } = null!;
    private List<ClientDto_RecivedInvitations> RecivedInvitationsList { get; set; } = [];

    private bool _processingRejectInvitation = false;
    private bool _processingAcceptInvitation = false;

    protected override async Task OnInitializedAsync()
    {
        var getSentInvitations = await InvitationService.GetShopsInvitations();

        if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
            RecivedInvitationsList = getSentInvitations.Value;
    }

    private async Task
    RejectInvitation
    (
        ClientDto_RecivedInvitations recivedInvitation
    )
    {
        _processingRejectInvitation = true;

        var rejectInvitationResult = await InvitationService.RejectInvitation(new ClientDto_InvitationRequestActions
        {
            ShopId = recivedInvitation.ShopId,
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
        ClientDto_RecivedInvitations recivedInvitation
    )
    {
        _processingAcceptInvitation = true;

        var acceptInvitationResult = await InvitationService.AcceptInvitation(new ClientDto_InvitationRequestActions
        {
            ShopId = recivedInvitation.ShopId,
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