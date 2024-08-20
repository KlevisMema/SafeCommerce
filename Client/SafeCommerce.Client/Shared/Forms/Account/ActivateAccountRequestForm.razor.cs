using MudBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Shared.Forms.Account;

public partial class ActivateAccountRequestForm
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!;

    private bool _processing = false;
    private EditForm? ActivateAccountEditForm;
    private ClienDto_ActivateAccountRequest? ActivateAccountRequest { get; set; } = new();

    private async Task
    ActivateAccountRequestValidation()
    {
        var validationPassed = ActivateAccountEditForm!.EditContext!.Validate();

        if (!validationPassed)
        {
            ShowValidationsMessages(ActivateAccountEditForm.EditContext.GetValidationMessages());
            return;
        }

        await SumbmitActivateAccountRequest();
    }

    private async Task
    SumbmitActivateAccountRequest()
    {
        _processing = true;
        await Task.Delay(1000);

        var requestResult = await _userManagment.ActivateAccountRequest(ActivateAccountRequest);

        if (!requestResult.Succsess)
            _snackbar.Add(requestResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
        else
            _snackbar.Add(requestResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });

        ActivateAccountRequest = new();
        _processing = false;
    }

    private void
    RedirectToLoginPage()
    {
        _navigationManager.NavigateTo("/");
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
