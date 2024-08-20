using MudBlazor;
using Microsoft.JSInterop;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Shop;

public partial class EditShopForm
{
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;

    [Parameter] public ClientDto_Shop ShopToBeEdited { get; set; }
    [Parameter] public EventCallback<ClientDto_Shop> OnShopUpdated { get; set; }

    private ClientDto_UpdateShop? UpdateShop { get; set; }

    private bool _processing = false;
    private EditForm? ChangeShopForm;

    protected override Task 
    OnInitializedAsync()
    {
        UpdateShop = new()
        {
            DataNonce = ShopToBeEdited.DataNonce,
            Description = ShopToBeEdited.Description,
            EncryptedKeyNonce = ShopToBeEdited.EncryptedKeyNonce,
            EncryptedSymmetricKey = ShopToBeEdited.EncryptedKey,
            Name = ShopToBeEdited.Name
        };

        return base.OnInitializedAsync();
    }

    private async Task
    ValidateForm()
    {
        var validationsPassed = ChangeShopForm!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(ChangeShopForm.EditContext.GetValidationMessages());
        else
            await SubmitUserDataUpdateForm();
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

    private async Task
    SubmitUserDataUpdateForm()
    {
        _processing = true;
        await Task.Delay(1000);

        if (ShopToBeEdited.IsPublic)
            await NormalUpdate(this.UpdateShop!);
        else
            await UpdateEncryptedShop();

        _processing = false;
        MudDialog.Close();

        await InvokeAsync(StateHasChanged);
    }

    private async Task 
    UpdateEncryptedShop()
    {
        var encryptedShop = await JsRuntime.InvokeAsync<CientDto_CreateShopWithAllKeys>("updateShop",
                UpdateShop,
                ShopToBeEdited.EncryptedKey,
                ShopToBeEdited.DataNonce,
                ShopToBeEdited.ShopId,
                ShopToBeEdited.OwnerId);

        var transformedShopData = new ClientDto_UpdateShop()
        {
            DataNonce = encryptedShop.DataNonce,
            Description = encryptedShop.Description,
            EncryptedKeyNonce = encryptedShop.EncryptedKeyNonce,
            EncryptedSymmetricKey = encryptedShop.EncryptedSymmetricKey,
            Name = encryptedShop.Name,
            SignatureOfKey = encryptedShop.SignatureOfKey,
            SigningPublicKey = encryptedShop.SigningPublicKey,
        };

        await NormalUpdate(transformedShopData);
    }

    private async Task 
    NormalUpdate
    (
        ClientDto_UpdateShop UpdateShop
    )
    {
        var updateDataResult = await ShopService.EditShop(ShopToBeEdited.ShopId, UpdateShop!);

        if (!updateDataResult.Succsess)
        {
            _snackbar.Add(updateDataResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });

            if (updateDataResult.Errors is not null)
                ShowValidationsMessages(updateDataResult.Errors);
        }
        else
        {
            _snackbar.Add(updateDataResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
            updateDataResult.Value!.Name = this.UpdateShop.Name;
            updateDataResult.Value.Description = this.UpdateShop.Description;
            await OnShopUpdated.InvokeAsync(updateDataResult.Value);

        }

    }
}