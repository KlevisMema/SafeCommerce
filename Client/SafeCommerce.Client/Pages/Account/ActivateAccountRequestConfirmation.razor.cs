using MudBlazor;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Pages.Account;

public partial class ActivateAccountRequestConfirmation
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!;

    private string? token;
    private string? email;
    private bool isVisible = true;
    private bool buttonDisabled = false;
    private ClientDto_ActivateAccountConfirmation? Dto_ActivateAccountConfirmation { get; set; }
    private string ActivateAccountMessage { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var uri = new Uri(_navigationManager.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

            token = query["token"];
            email = query["email"];

            Dto_ActivateAccountConfirmation = new()
            {
                Token = token ?? string.Empty,
                Email = email ?? string.Empty

            };

            var activateAccountRequestConfirmationResult = await _userManagment.ActivateAccountRequestConfirmation(Dto_ActivateAccountConfirmation);

            isVisible = false;

            if (!activateAccountRequestConfirmationResult.Succsess)
            {
                buttonDisabled = false;
                ActivateAccountMessage = "Account was not activated, if this message persists please request another account activation.";
                _snackbar.Add(activateAccountRequestConfirmationResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
            }
            else
            {
                buttonDisabled = true;
                ActivateAccountMessage = "Your account was succsessfully activated, please log in the application.";
                _snackbar.Add("Redirecting you in the login page.", Severity.Success, config => { config.CloseAfterNavigation = true; });
                await InvokeAsync(StateHasChanged);
                await Task.Delay(3000);
                _navigationManager.NavigateTo("/");
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    private void RedirectToRequestAccountConfirmation()
    {
        _navigationManager?.NavigateTo("/Profile/Activate");
    }
}