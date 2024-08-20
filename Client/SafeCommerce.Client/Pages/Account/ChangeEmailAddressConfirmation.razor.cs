using MudBlazor;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Pages.Account;

public partial class ChangeEmailAddressConfirmation
{
    [Inject] private AppState _appState { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagmentService { get; set; } = null!;

    private string? token;
    private string? email;
    private bool isVisible = true;
    private string ActivateAccountMessage { get; set; } = string.Empty;

    private ClientDto_ChangeEmailAddressRequestConfirm? ClientDto_ChangeEmailRequestConfirm { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var uri = new Uri(_navigationManager.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

            token = query["token"];
            email = query["email"];

            ClientDto_ChangeEmailRequestConfirm = new()
            {
                Token = token ?? string.Empty,
                EmailAddress = email ?? string.Empty

            };

            var confirmChangeEmailAddressRequest = await _userManagmentService.ConfirmChangeEmailAddressRequest(ClientDto_ChangeEmailRequestConfirm);

            isVisible = false;

            if (!confirmChangeEmailAddressRequest.Succsess)
            {
                ActivateAccountMessage = "New email was not confirmed, if this message persists please make another request!!";
                _snackbar.Add(confirmChangeEmailAddressRequest.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
            }
            else
            {
                ActivateAccountMessage = "Your email was succsessfully changed, you may close this page";
                _snackbar.Add(confirmChangeEmailAddressRequest.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
            }

            await InvokeAsync(StateHasChanged);
        }
    }
}