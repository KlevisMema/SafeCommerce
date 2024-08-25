using MudBlazor;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Enums;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.Client.Shared.Forms.Account;

namespace SafeCommerce.Client.Shared;

public partial class NavMenu
{
    [Parameter] public bool DataRetrieved { get; set; }
    [Inject] private AppState _appState { get; set; } = null!;
    [Parameter] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] NavigationManager _navigationManager { get; set; } = null!;
    [Inject] IAuthenticationService AuthenticationService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private Role UserRole { get; set; }

    protected override async Task
    OnInitializedAsync()
    {
        this.UserRole = await UserUtilities.GetRole(LocalStorageService);
    }

    private async Task
    OpenPopUpDeactivateAccountForm()
    {
        var dialog = await DialogService.ShowAsync<DeactivateAccount>("Deactivate Account Dialog", DialogHelper.DialogOptions());
        await dialog.Result;
    }

    private async Task
    OpenPopUpChangeEmailForm()
    {
        var dialog = await DialogService.ShowAsync<RequestChangeEmailAddress>("Change Email Dialog", DialogHelper.DialogOptions());
        await dialog.Result;
    }

    private async Task
    OpenPopUpChangePasswordForm()
    {
        var dialog = await DialogService.ShowAsync<ChangePassword>("Change Password Dialog", DialogHelper.DialogOptions());
        await dialog.Result;
    }
}