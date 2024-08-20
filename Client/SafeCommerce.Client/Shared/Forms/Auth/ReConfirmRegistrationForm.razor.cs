using MudBlazor;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class ReConfirmRegistrationForm
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;

    private bool _processing = false;
    private EditForm? ReConfirmRegistration;
    private ClientDto_ReConfirmRegistration ConfirmRegistration { get; set; } = new();

    private async Task
    ValidateReConfirmRegistration()
    {
        var validationPassed = ReConfirmRegistration!.EditContext!.Validate();

        if (!validationPassed)
        {
            ShowValidationsMessages(ReConfirmRegistration.EditContext.GetValidationMessages());
            return;
        }

        await SubmitReConfirmRegistrationForm();
    }

    private async Task
    SubmitReConfirmRegistrationForm()
    {
        _processing = true;
        await Task.Delay(1000);

        var registerResult = await _authenticationService.ReConfirmRegistrationRequest(ConfirmRegistration);

        if (!registerResult.Succsess)
            _snackbar.Add(registerResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
        else
            _snackbar.Add(registerResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });

        ConfirmRegistration = new();
        _processing = false;
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages
    )
    {
        foreach (var validationMessage in validationMessages)
            _snackbar.Add(validationMessage, Severity.Warning, config => { config.CloseAfterNavigation = true; });
    }

    private void
    RedirectToLoginPage()
    {
        _navigationManager.NavigateTo("/Login");
    }
}