using MudBlazor;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.AccountManagment;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class ForgotPasswordForm
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!; 

    private EditForm? ForgotPasswordEditForm;
    private bool _processing = false;

    private ClientDto_ForgotPassword ForgotPassword { get; set; } = new();

    private async Task
    ForgotPasswordRequest()
    {
        var validationPassed = ForgotPasswordEditForm!.EditContext!.Validate();

        if (!validationPassed)
        {
            ShowValidationsMessages(ForgotPasswordEditForm.EditContext.GetValidationMessages());
            return;
        }

        await SumbmitForgotPasswordRequest();
    }

    private async Task
    SumbmitForgotPasswordRequest()
    {
        _processing = true;
        await Task.Delay(1000);

        var requestResult = await _userManagment.ForgotPassword(ForgotPassword);

        if (!requestResult.Succsess)
            _snackbar.Add(requestResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
        else
            _snackbar.Add(requestResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });

        ForgotPassword = new();
        _processing = false;
    }

    private void
    RedirectToLoginPage()
    {
        _navigationManager.NavigateTo("/Login");
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
}