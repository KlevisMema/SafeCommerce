using MudBlazor;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Pages.Auth;

public partial class ActivateAccount
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;

    private string? token;
    private string? email;
    private bool isVisible = true;
    private bool buttonDisabled = false;
    private ClientDto_ConfirmRegistration? ConfirmRegistration { get; set; }
    private string ActivateAccountMessage { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var uri = new Uri(_navigationManager.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

            token = query["token"];
            email = query["email"];

            ConfirmRegistration = new()
            {
                Token = token ?? string.Empty,
                Email = email ?? string.Empty

            };

            var confirmRegistrationResult = await _authenticationService.ConfirmUserRegistration(ConfirmRegistration);

            isVisible = false;

            if (!confirmRegistrationResult.Succsess)
            {
                buttonDisabled = false;
                ActivateAccountMessage = "Registration was not confirmed, if this message persists please request another account confirmation.";
                _snackbar.Add(confirmRegistrationResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
            }
            else
            {
                buttonDisabled = true;
                ActivateAccountMessage = "Your account was succsessfully activated, please log in the application.";
                _snackbar.Add("Redirecting you in the login page.", Severity.Success, config => { config.CloseAfterNavigation = true; });
                await InvokeAsync(StateHasChanged);
                await Task.Delay(3000);
                _navigationManager.NavigateTo("/Login");
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    private void RedirectToRequestAccountConfirmation()
    {
        if (_navigationManager != null)
        {
            _navigationManager.NavigateTo("/Authentication/ReConfirmRegistration");
        }
    }
}