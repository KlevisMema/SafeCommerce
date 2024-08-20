using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Authentication;
using SafeCommerce.ClientServices.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientDTO.User;
using SafeCommerce.ClientDTO.AccountManagment;
using SafeCommerce.Client.Internal.Helpers;

namespace SafeCommerce.Client.Shared.Forms.Auth;

public partial class KeysGeneration
{
    [Inject] private AppState AppState { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime _jsInterop { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorage { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;

    [Parameter] public ClientDto_UserInfo UserInfo { get; set; }

    private bool _processing = false;
    private EditForm? CreateSecretPassPhraseForm;
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    private ClientDto_PassPhrase dto_PassPhrase { get; set; } = new();

    private const string GeneralFailKeys = "Something went wrong, please try again!";
    private const string EndOfOperation = "Operation finished, we invite you to log in again!";
    private const string KeysGenerated = "Keys successfully generated and stored in your browser!";
    private const string KeysGeneratedError = "Your secret passphrase its not correct!";

    private const string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
    private string _dragClass = DefaultDragClass;
    private string _fileName = string.Empty;
    private MudFileUpload<IBrowserFile>? _fileUpload;
    private IBrowserFile? _uploadedFile;

    private async Task
    ValidateFormOfUserGivingPassPhraseFirsTime()
    {
        var validationsPassed = CreateSecretPassPhraseForm!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(CreateSecretPassPhraseForm.EditContext.GetValidationMessages());
        else
            await SubmitCreateKeysForm();
    }

    private async Task
    ValidateFormOfUserHavingKeyInServerNotInBrowser()
    {
        var validationsPassed = CreateSecretPassPhraseForm!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(CreateSecretPassPhraseForm.EditContext.GetValidationMessages());
        else
            await CreateKeys();
    }

    private async Task<string>
    GetFileContentAsString
    (
        IBrowserFile file
    )
    {
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    private async Task
    CreateKeys()
    {
        if (_uploadedFile is not null)
        {
            string? userId = await _localStorage.GetItemAsStringAsync("Id");
            string encryptedfileContent = await GetFileContentAsString(_uploadedFile);
            var decryptedKeyPairJson = await _jsInterop.InvokeAsync<bool>("decryptAndSaveKeyPair", encryptedfileContent, dto_PassPhrase.SecretPassPhrase, dto_PassPhrase.MyDevice, userId);

            if (!decryptedKeyPairJson)
            {
                _snackbar.Add("The file or the passphrase is not correct", Severity.Error, config =>
                {
                    config.CloseAfterNavigation = true;
                    config.VisibleStateDuration = 3000;
                });

                return;
            }

            if (UserInfo is not null)
            {
                bool isValidPublicKey = await _jsInterop.InvokeAsync<bool>
                (
                    "verifyPublicKey",
                    UserInfo.PublicKey,
                    UserInfo.SigningPublicKey,
                    UserInfo.Signature
                );

                if (!isValidPublicKey)
                {
                    await _jsInterop.InvokeVoidAsync("deleteKeyFromDatabase", userId);
                    _snackbar.Add("Someting went wrong", Severity.Error, config => { config.CloseAfterNavigation = true; });
                    await LogOutHelper.LogOut(AppState, _navigationManager, _localStorage, _authenticationService);

                    return;
                }
            }

            _snackbar.Add(KeysGenerated, Severity.Success, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });


            MudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            await GenerateKeys(false);
        }
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages
    )
    {
        foreach (var validationMessage in validationMessages)
        {
            _snackbar.Add(validationMessage, Severity.Error, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });
        }
    }

    private async Task
    SubmitCreateKeysForm()
    {
        _processing = true;

        await GenerateKeys(true);

        _processing = false;
        MudDialog.Close();
    }

    private async Task GenerateKeys(bool storeInServer)
    {
        string? userId = await _localStorage.GetItemAsStringAsync("Id");

        bool successGenerateKeys = await _jsInterop.InvokeAsync<bool>("generateAndStoreKeys", dto_PassPhrase.SecretPassPhrase, userId, dto_PassPhrase.MyDevice);

        if (!successGenerateKeys)
        {
            _snackbar.Add(GeneralFailKeys, Severity.Error, options =>
            {
                options.CloseAfterNavigation = true;
            });
            _processing = false;

            return;
        }

        if (storeInServer)
        {
            var result = await StorePublicKeyInServer(userId);

            if (!result)
                return;
        }
        else
        {
            string? publicKey = await _jsInterop.InvokeAsync<string>("getPublicKey", userId);

            bool isSamePk = await VerifyPublickey(publicKey);

            if (!isSamePk)
            {
                _snackbar.Add(KeysGeneratedError, Severity.Error, options =>
                {
                    options.CloseAfterNavigation = true;
                });

                return;
            }
        }

        _snackbar.Add(KeysGenerated, Severity.Success, options =>
        {
            options.CloseAfterNavigation = true;
        });

        MudDialog.Close(DialogResult.Ok(true));
    }

    private async Task<bool>
    VerifyPublickey
    (
        string generatedPublicKey
    )
    {
        if (UserInfo is not null)
            return generatedPublicKey == UserInfo.PublicKey;

        var getUserInfo = await _userManagment.GetUser();

        if (getUserInfo.Succsess)
            return getUserInfo.Value!.PublicKey == generatedPublicKey;

        return true;
    }

    private async Task<bool> StorePublicKeyInServer
    (
        string userId
    )
    {
        string? publicKey = await _jsInterop.InvokeAsync<string>("getPublicKey", userId);
        string? signature = await _jsInterop.InvokeAsync<string>("getSignature", userId);
        string? signingPublicKey = await _jsInterop.InvokeAsync<string>("getSigningPublicKey", userId);

        var savePublicKey = await _authenticationService.SaveUserPublicKey(userId, new ClientDto_SavePublicKey
        {
            HintPassPhrase = dto_PassPhrase.Hint,
            PublicKey = publicKey,
            SigningPublicKey = signingPublicKey,
            Signature = signature,
        });

        if (!savePublicKey.Succsess)
        {
            _snackbar.Add(savePublicKey.Message, Severity.Error, options =>
            {
                options.CloseAfterNavigation = true;
            });
            _processing = false;
            return false;
        }
        return true;
    }

    private async Task ClearAsync()
    {
        _uploadedFile = null;

        _fileName = string.Empty;

        ClearDragClass();
    }

    private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();
        var files = e.GetMultipleFiles();

        foreach (var file in files)
        {
            if (!IsImageValidFile(file.ContentType))
            {
                IEnumerable<string> errors =
                [
                    $"Invalid .{file.ContentType.Split("/")[1]} file format",
                ];

                ShowValidationsMessages(errors);

                _fileName = string.Empty;
            }
            else
                _fileName = file.Name;
        }
    }

    private void FileUploaded(IBrowserFile file)
    {
        if (!IsImageValidFile(file.ContentType))
        {
            IEnumerable<string> errors =
            [
                $"Invalid .{file.ContentType.Split("/")[1]} file format",
            ];

            ShowValidationsMessages(errors);
            return;
        }

        this._uploadedFile = file;
    }

    private static bool
    IsImageValidFile
    (
        string contentType
    )
    {
        var allowedTypes = new[] { "application/json" };
        return allowedTypes.Contains(contentType);
    }

    private Task OpenFilePickerAsync()
        => _fileUpload?.OpenFilePickerAsync() ?? Task.CompletedTask;

    private void SetDragClass()
        => _dragClass = $"{DefaultDragClass} mud-border-primary";

    private void ClearDragClass()
        => _dragClass = DefaultDragClass;
}