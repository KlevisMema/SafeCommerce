#region Usings
using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.Client.Internal;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientUtilities.Responses;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.Client.Shared.Forms.Account;
using System.Runtime.InteropServices.JavaScript;
using SafeCommerce.ClientDTO.User;
#endregion

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class LoginForm
{
    #region Injections
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private IJSRuntime _jsInterop { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorage { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;
    #endregion

    #region Other properties
    private EditForm? loginForm;
    private EditForm? registerForm;

    private bool _processing = false;
    private bool _processingRegistering = false;
    private ClientDto_Login clientDto_Login { get; set; } = new();
    private ClientDto_Register clientDto_Register { get; set; } = new();
    private bool showActivateAccountRequestBtn { get; set; } = false;
    #endregion

    #region Constants
    private const string SnackbarMessage = "Redirecting you in the dashboard page";
    private const string SnackbarMessage1 = "Credentials are being validated";
    private const string SnackbarMessage2 = "Credentials are being created";
    private const string LoginMode = "Login";
    private const string RegisterMode = "Register";
    private const string RegisterModeInvalidGender = "Register - Please select a valid gender";
    private const string OtpSendMessage = "An email with the otp has been sent you!";
    private const string OtpRedirection = "Redirecting you to the otp validation!";
    private const string GeneralFailKeys = "Something went wrong, please try again!";
    #endregion

    #region Functions

    protected override async Task
    OnInitializedAsync()
    {
        showActivateAccountRequestBtn = false;

        if (await _localStorage.GetItemAsync<bool>("SessionExpired"))
        {
            _snackbar.Add("Session Expired!", Severity.Error, config => { config.CloseAfterNavigation = false; });
            await _localStorage.RemoveItemAsync("SessionExpired");
            await InvokeAsync(StateHasChanged);
            clientDto_Login = new();
        }
    }

    private async Task
    SubmitLoginForm()
    {
        if (showActivateAccountRequestBtn)
            showActivateAccountRequestBtn = false;

        _snackbar.Add(SnackbarMessage1, Severity.Info, config => { config.CloseAfterNavigation = true; });

        _processing = true;
        await Task.Delay(1000);

        var loginResult = await _authenticationService.LogInUser(clientDto_Login);

        if (loginResult.Succsess && loginResult.Value is not null)
            await LoginSuccess(loginResult);
        else
            UnsuccessfulLogIn(loginResult.Message, loginResult.Message!.Contains("deactivated"));

        _processing = false;
    }

    private async Task LoginSuccess
    (
        ClientUtil_ApiResponse<ClientDto_LoginResult> loginResult
    )
    {
        await _localStorage.SetItemAsStringAsync("Id", loginResult.Value!.UserId);
        await _localStorage.SetItemAsStringAsync("Role", loginResult.Value!.UserRole);

        if (loginResult.Value.GenerateKeys)
            await OpenPopUpGiveSecretPassPhrase(loginResult.Value!.UserId);
        else
            await CheckIfKeysExistInBrowser(loginResult.Value.UserId);

        if (loginResult.Value.RequireOtpDuringLogin)
        {
            await RedirectToOtpValidationPage(loginResult.Value.UserId);
            return;
        }

        await _localStorage.SetItemAsStringAsync("FullName", loginResult.Value.UserFullName);

        await RedirectToDashboardPage(loginResult.Message, loginResult.Value);
    }

    private void UnsuccessfulLogIn
    (
        string? message,
        bool accountDeactivated
    )
    {
        _snackbar.Add(message ?? "Something went wrong, try again", Severity.Error, options =>
        {
            options.CloseAfterNavigation = true;
        });

        if (accountDeactivated)
            showActivateAccountRequestBtn = true;
    }

    private async Task
    CheckIfKeysExistInBrowser
    (
        string userId
    )
    {
        string? publicKey = await _jsInterop.InvokeAsync<string>("getPublicKey", userId);

        if (string.IsNullOrEmpty(publicKey))
        {
            AppState.GenerateSameKeys = true;
            await OpenPopUpGiveSecretPassPhrase(userId);
        }

    }

    private async Task
    OpenPopUpGiveSecretPassPhrase
    (
        string userId
    )
    {
        string? publicKey = await _jsInterop.InvokeAsync<string>("getPublicKey", userId);
        string? privateKey = await _jsInterop.InvokeAsync<string>("getPrivateKey", userId);
        string? signature = await _jsInterop.InvokeAsync<string>("getSignature", userId);
        string? signingPublicKey = await _jsInterop.InvokeAsync<string>("getSigningPublicKey", userId);

        if (!String.IsNullOrEmpty(publicKey) && !String.IsNullOrEmpty(privateKey) && !String.IsNullOrEmpty(signature))
        {
            var savePublicKey = await _authenticationService.SaveUserPublicKey(userId, new ClientDto_SavePublicKey
            {
                HintPassPhrase = string.Empty,
                PublicKey = publicKey,
                Signature = signature,
                SigningPublicKey = signingPublicKey
            });

            return;
        }

        var dialog = await DialogService.ShowAsync<KeysGeneration>("Enter secret passphrase", DialogHelper.DialogOptionsNoCloseButton());
        await dialog.Result;
    }

    private async Task
    RedirectToOtpValidationPage
    (
        string userId
    )
    {
        _snackbar.Add(OtpSendMessage, Severity.Info, config => { config.CloseAfterNavigation = true; });
        await Task.Delay(2000);
        _navigationManager.NavigateTo($"/Authentication/2FA/{userId}");
    }

    private async Task
    RedirectToDashboardPage
    (
        string? loginResultMessage,
        ClientDto_LoginResult loginResult
    )
    {
        AppState.SetClientSecrets(loginResult);
        _snackbar.Add(loginResultMessage, Severity.Success, config => { config.CloseAfterNavigation = true; });
        await Task.Delay(2000);
        _navigationManager.NavigateTo("/Dashboard");
    }

    private async Task
    SubmitRegistrationForm()
    {
        _snackbar.Add(SnackbarMessage2, Severity.Info, config => { config.CloseAfterNavigation = true; });

        _processingRegistering = true;
        await Task.Delay(1000);

        var registerResult = await _authenticationService.RegisterUser(clientDto_Register);

        if (!registerResult.Succsess)
        {
            _snackbar.Add(registerResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });
            ShowValidationsMessages(registerResult.Errors!, RegisterMode, true);
        }
        else
            _snackbar.Add(registerResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });

        clientDto_Register = new();
        _processingRegistering = false;
    }

    private async Task
    ValidateForm()
    {
        var validationPassed = loginForm!.EditContext!.Validate();

        if (validationPassed)
            await SubmitLoginForm();
        else
            ShowValidationsMessages(loginForm.EditContext.GetValidationMessages(), LoginMode, false);
    }

    private async Task
    ValidateRegisterForm()
    {
        var validationPassed = registerForm!.EditContext!.Validate();

        if (validationPassed)
        {

            if (clientDto_Register.Gender == ClientDTO.Enums.Gender.SelectGender)
            {
                _snackbar.Add(RegisterModeInvalidGender, Severity.Warning, config => { config.CloseAfterNavigation = true; });
                return;
            }

            var passwordValidations = PasswordStrength(clientDto_Register.Password);

            if (passwordValidations.Any())
            {
                ShowValidationsMessages(passwordValidations, RegisterMode, false);
                return;
            }

            await SubmitRegistrationForm();
        }
        else
            ShowValidationsMessages(registerForm.EditContext.GetValidationMessages(), RegisterMode, false);
    }

    private async Task
    ToggleForms()
    {
        await _jsInterop.InvokeVoidAsync("LoginPage");
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages,
        string mode,
        bool fromServer
    )
    {
        if (fromServer)
        {
            foreach (var validationMessage in validationMessages)
            {
                _snackbar.Add(validationMessage, Severity.Warning, config => { config.CloseAfterNavigation = true; config.VisibleStateDuration = 3000; });
            }

            return;
        }

        foreach (var validationMessage in validationMessages)
        {
            _snackbar.Add($"{mode} - {validationMessage}", Severity.Warning, config => { config.CloseAfterNavigation = true; });
        }
    }

    private static IEnumerable<string>
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
    NavigateToForgotPasswordPage()
    {
        _navigationManager.NavigateTo("/UserManagment/ForgotPassword");
    }

    [GeneratedRegex(@"[A-Z]")]
    private static partial Regex MyRegex();
    [GeneratedRegex(@"[a-z]")]
    private static partial Regex MyRegex1();
    [GeneratedRegex(@"[0-9]")]
    private static partial Regex MyRegex2();
    [GeneratedRegex(@"[\p{P}\p{S}]")]
    private static partial Regex MyRegex3();

    #endregion
}