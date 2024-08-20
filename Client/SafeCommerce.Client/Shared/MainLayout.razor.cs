using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.User;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.Client.Shared.Forms.Auth;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;
using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.Client.Shared;

public partial class MainLayout
{
    [Inject] private AppState _appState { get; set; } = null!;
    [Inject] private IJSRuntime jsruntime { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IAuthenticationService authService { get; set; } = null!;
    [Inject] private ILocalStorageService _localStorage { get; set; } = null!;
    [Inject] IClientService_Invitation InvitationService { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IClientService_UserManagment _userManagment { get; set; } = null!;
    [Inject] private IAuthenticationService _authenticationService { get; set; } = null!;

    private List<ClientDto_RecivedInvitations> RecivedInvitationsList { get; set; } = [];


    private bool _open;
    private bool _drawerOpen = true;
    public bool DataRetrieved { get; set; } = true;
    private bool hideLogOutBtn { get; set; } = false;
    private string LogOutText { get; set; } = "Log Out";
    private string NotificationsText { get; set; } = "Notifications";
    private Role UserRole { get; set; }

    protected override async Task
    OnInitializedAsync()
    {
        string? userId = await _localStorage.GetItemAsStringAsync("Id");

        if (userId == null)
        {
            _snackbar.Add("Something wen't wrong", Severity.Error, config => { config.CloseAfterNavigation = false; });
            await LogoutUser();

            return;
        }

        this.UserRole = await UserUtilities.GetRole(_localStorage);

        string? publicKey = await jsruntime.InvokeAsync<string>("getPublicKey", userId);
        string? privateKey = await jsruntime.InvokeAsync<string>("getPrivateKey", userId);
        string? signingPublicKey = await jsruntime.InvokeAsync<string>("getSigningPublicKey", userId);
        string? signingPrivateKey = await jsruntime.InvokeAsync<string>("getSigningPrivateKey", userId);
        string? signature = await jsruntime.InvokeAsync<string>("getSignature", userId);


        var getUserInfo = await _userManagment.GetUser();

        if (getUserInfo is null || getUserInfo.Value == null)
        {
            _snackbar.Add("Someting went wrong, logging you out", Severity.Error, config => { config.CloseAfterNavigation = false; });

            await LogoutUser();

            await _authenticationService.LogoutUser();

            return;
        }

        if
        (
            String.IsNullOrEmpty(publicKey) &&
            String.IsNullOrEmpty(privateKey) &&
            String.IsNullOrEmpty(signingPublicKey) &&
            String.IsNullOrEmpty(signingPrivateKey) &&
            String.IsNullOrEmpty(signature)
        )
        {
            if (getUserInfo.Succsess && getUserInfo.Value != null && String.IsNullOrEmpty(getUserInfo.Value.PublicKey))
            {
                _appState.GenerateSameKeys = false;
                var dialog = await DialogService.ShowAsync<KeysGeneration>("Enter secret passphrase", DialogHelper.DialogOptionsNoCloseButton());
                await dialog.Result;
            }
            else
            {
                _appState.GenerateSameKeys = true;

                var parameters = new DialogParameters<ClientDto_UserInfo> { { "UserInfo", getUserInfo.Value } };

                var dialog = await DialogService.ShowAsync<KeysGeneration>("Enter secret passphrase", parameters, DialogHelper.DialogOptionsNoCloseButton());
                await dialog.Result;
            }
        }

        if (!String.IsNullOrEmpty(publicKey) && !String.IsNullOrEmpty(publicKey) && String.IsNullOrEmpty(getUserInfo!.Value!.PublicKey))
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
        else
        {
            if (getUserInfo.Value.PublicKey == "" || getUserInfo.Value.PublicKey == "" || getUserInfo.Value.PublicKey == "")
                getUserInfo = await _userManagment.GetUser();

            bool isValidPublicKey = await jsruntime.InvokeAsync<bool>
            (
                "verifyPublicKey",
                getUserInfo.Value.PublicKey,
                getUserInfo.Value.SigningPublicKey,
                getUserInfo.Value.Signature
            );

            if (!isValidPublicKey)
            {
                _snackbar.Add("Someting went wrong", Severity.Error, config => { config.CloseAfterNavigation = true; });
            }
        }

        publicKey = await jsruntime.InvokeAsync<string>("getPublicKey", userId);

        if (publicKey != getUserInfo.Value.PublicKey)
        {
            _snackbar.Add("Keys miss match, restore them", Severity.Error, options =>
            {
                options.CloseAfterNavigation = false;
            });

            _appState.GenerateSameKeys = true;

            var parameters = new DialogParameters<ClientDto_UserInfo> { { "UserInfo", getUserInfo.Value } };

            var dialog = await DialogService.ShowAsync<KeysGeneration>("Enter secret passphrase", parameters, DialogHelper.DialogOptionsNoCloseButton());
            await dialog.Result;
        }

        var role = await _localStorage.GetItemAsStringAsync("Role");

        if (role == Role.User.ToString())
        {
            var getSentInvitations = await InvitationService.GetShopsInvitations();

            if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
                RecivedInvitationsList = getSentInvitations.Value;
        }

        DataRetrieved = true;
        return;
    }

    private async Task
    LogoutUser()
    {
        if (await _localStorage.ContainKeyAsync("SessionExpired"))
            await _localStorage.RemoveItemAsync("SessionExpired");

        if (await _localStorage.ContainKeyAsync("FullName"))
            await _localStorage.RemoveItemAsync("FullName");

        if (await _localStorage.ContainKeyAsync("Id"))
            await _localStorage.RemoveItemAsync("Id");

        if (await _localStorage.ContainKeyAsync("Role"))
            await _localStorage.RemoveItemAsync("Role");

        _appState.LogOut();

        LogOutText = "Logging Out";
        hideLogOutBtn = true;
        _snackbar.Add("Logging you out", Severity.Success, config => { config.CloseAfterNavigation = true; });
        await Task.Delay(2000);
        await authService.LogoutUser();
        await _localStorage.RemoveItemAsync("UserData");
        _navigationManager.NavigateTo("/");
        hideLogOutBtn = false;
        LogOutText = "Log Out";
    }

    private void
    DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task
    OpenNotifications()
    {
        var getSentInvitations = await InvitationService.GetShopsInvitations();

        if (getSentInvitations != null && getSentInvitations.Succsess && getSentInvitations.Value is not null)
            RecivedInvitationsList = getSentInvitations.Value;

        ToggleOpen();
    }

    public void ToggleOpen()
    {
        if (_open)
            _open = false;
        else
            _open = true;
    }
}