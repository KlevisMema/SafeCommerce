using MudBlazor;
using System.Net;
using Blazored.LocalStorage;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class TwoFaForm
{
    [Parameter] public string? userId { get; set; }
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorage { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;

    private EditForm? TwoFAForm;
    private bool _processing = false;
    private string heightStyle = "300px";
    private ClientDto_2FA TwoFA { get; set; } = new();
    private HttpStatusCode HttpStatusCode { get; set; }

    private const string SnackbarMessage = "Redirecting you in the dashboard page";

    private async Task
    ConfirmLogin2FA()
    {
        var validationPassed = TwoFAForm!.EditContext!.Validate();

        if (!validationPassed)
        {
            ShowValidationsMessages(TwoFAForm.EditContext.GetValidationMessages());
            return;
        }

        await ConfirmLogin2FASubmit();
    }

    private async Task ConfirmLogin2FASubmit()
    {
        ShowSnackbar("Validating the OTP", Severity.Info);
        _processing = true;
        await Task.Delay(1000);

        if (!TryParseUserId(out Guid parsedUserId))
        {
            HandleUserIdParsingError();
            return;
        }

        TwoFA.UserId = parsedUserId;

        var twoFaResult = await _authenticationService.ConfirmLogin2FA(TwoFA);
        if (!twoFaResult.Succsess)
        {
            HandleTwoFAResultFailure(twoFaResult);
            return;
        }

        await HandleTwoFAResultSuccess(twoFaResult);
    }

    private void ShowSnackbar(string message, Severity severity)
    {
        _snackbar.Add(message, severity, config => { config.CloseAfterNavigation = true; });
    }

    private bool TryParseUserId(out Guid userId)
    {
        try
        {
            userId = Guid.Parse(this.userId);
            return true;
        }
        catch (Exception)
        {
            userId = default;
            return false;
        }
    }

    private void HandleUserIdParsingError()
    {
        ShowSnackbar("An unexpected error happened, try again", Severity.Error);
        heightStyle = "350px";
        HttpStatusCode = HttpStatusCode.InternalServerError;
        TwoFA = new();
        _processing = false;
    }

    private void HandleTwoFAResultFailure(ClientUtil_ApiResponse<ClientDto_LoginResult> result)
    {
        string message = String.IsNullOrEmpty(result.Message) ?
                         "An unexpected error happened" : result.Message;
        Severity severity = result.StatusCode == HttpStatusCode.InternalServerError ?
                            Severity.Error : Severity.Warning;

        if (result.StatusCode == HttpStatusCode.InternalServerError)
            heightStyle = "350px";

        HttpStatusCode = result.StatusCode;

        ShowSnackbar(message, severity);
        TwoFA = new();
        _processing = false;
    }

    private async Task HandleTwoFAResultSuccess(ClientUtil_ApiResponse<ClientDto_LoginResult> result)
    {
        AppState.SetClientSecrets(result.Value);
        ShowSnackbar(result.Message, Severity.Success);
        await Task.Delay(2000);
        ShowSnackbar($"{SnackbarMessage}", Severity.Info);
        await Task.Delay(500);
        await _localStorage.SetItemAsStringAsync("UserData", result.Value!.UserId);
        _navigationManager.NavigateTo("/Dashboard");
        TwoFA = new();
        _processing = false;
        await InvokeAsync(StateHasChanged);
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