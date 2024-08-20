using MudBlazor;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;
using Microsoft.AspNetCore.Components.Forms;

namespace SafeCommerce.Client.Shared.Forms.Account;

public partial class DeactivateAccount
{

    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorage { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagmentService { get; set; } = null!;

    private ClientDto_DeactivateAccount Dto_DeactivateAccount { get; set; } = new();
    private EditForm? deactivateAccountForm;
    private bool _processing = false;
    private InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    bool isShow;

    private async Task
    ValidateForm()
    {
        var validationsPassed = deactivateAccountForm!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(deactivateAccountForm.EditContext.GetValidationMessages());
        else
            await SubmitUserDataUpdateForm();
    }

    private async Task
    SubmitUserDataUpdateForm()
    {
        _processing = true;
        await Task.Delay(1000);

        var updateDataResult = await _userManagmentService.DeactivateAccount(Dto_DeactivateAccount);

        if (!updateDataResult.Succsess)
        {
            _snackbar.Add(updateDataResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });

            if (updateDataResult.Errors is not null)
                ShowValidationsMessages(updateDataResult.Errors);

            Dto_DeactivateAccount = new();
            _processing = false;
        }
        else
        {
            if (await _localStorage.ContainKeyAsync("FullName"))
                await _localStorage.RemoveItemAsync("FullName");

            _snackbar.Add(updateDataResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
            _snackbar.Add("Logging you out", Severity.Success, config => { config.CloseAfterNavigation = true; });
            await Task.Delay(2000);
            Dto_DeactivateAccount = new();
            _processing = false;
            _navigationManager.NavigateTo("/", true);
        }

        MudDialog.Close();
        await InvokeAsync(StateHasChanged);
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages
    )
    {
        foreach (var validationMessage in validationMessages)
        {
            _snackbar.Add(validationMessage, Severity.Warning, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
        }
    }

    private void
    DisplayTxtPassword()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else
        {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
}