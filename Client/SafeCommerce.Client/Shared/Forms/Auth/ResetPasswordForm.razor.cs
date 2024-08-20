using MudBlazor;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class ResetPasswordForm
{
    [Parameter] public string? Token { get; set; }
    [Parameter] public string? Email { get; set; }
    [Parameter] public NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!;

    private ClientDto_ResetPassword ResetPassword { get; set; } = new();

    private EditForm? ResetPasswordEditFrom;
    private bool _processing = false;

    protected override void OnInitialized()
    {
        if (String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Token))
            _navigationManager.NavigateTo("/");

        base.OnInitialized();
    }

    private async Task
    ValidateRestorePassword()
    {
        ResetPassword.Token = Token!;
        ResetPassword.Email = Email!;

        var validationPassed = ResetPasswordEditFrom!.EditContext!.Validate();

        if (!validationPassed)
        {
            ShowValidationsMessages(ResetPasswordEditFrom.EditContext.GetValidationMessages());
            return;
        }

        var passwordValidations = PasswordStrength(ResetPassword.NewPassword);

        if (passwordValidations.Any())
        {
            ShowValidationsMessages(passwordValidations);
            return;
        }

        await SubmitValidateRestorePasswordForm();
    }

    private async Task
    SubmitValidateRestorePasswordForm()
    {
        _processing = true;
        await Task.Delay(1000);

        var restorePassword = await _userManagment.ResetPassword(ResetPassword);

        if (!restorePassword.Succsess && restorePassword.Errors is not null)
        {
            _snackbar.Add(restorePassword.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
            ShowValidationsMessages(restorePassword.Errors);
        }
        else
        {
            _snackbar.Add(restorePassword.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
        }

        ResetPassword = new();
        _processing = false;
    }

    private IEnumerable<string> 
    PasswordStrength
    (
        string pw
    )
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }
        if (pw.Length < 6)
            yield return "Password must be at least 6 characters long";
        if (!MyRegex().IsMatch(pw))
            yield return "Password must contain at least one uppercase letter";
        if (!MyRegex1().IsMatch(pw))
            yield return "Password must contain at least one lowercase letter";
        if (!MyRegex2().IsMatch(pw))
            yield return "Password must contain at least one digit";
        if (!MyRegex3().IsMatch(pw))
            yield return "Password must contain at least one special character";
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

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"[a-z]")]
    private static partial Regex MyRegex1();
    [GeneratedRegex(@"[0-9]")]
    private static partial Regex MyRegex2();
    [GeneratedRegex(@"[\p{P}\p{S}]")]
    private static partial Regex MyRegex3();
}