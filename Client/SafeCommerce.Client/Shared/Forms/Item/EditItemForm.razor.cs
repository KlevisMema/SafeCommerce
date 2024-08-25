using MudBlazor;
using Microsoft.JSInterop;
using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Item;

public partial class EditItemForm
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Item ItemService { get; set; } = null!;

    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public ClientDto_Item? ItemToBeEdited { get; set; }
    [Parameter] public EventCallback<ClientDto_Item> OnItemUpdated { get; set; }

    private string? imagePreview;
    private EditForm? UpdateItemForm;
    private bool _processing = false;
    private IBrowserFile? ImageFile { get; set; }
    private ClientDto_UpdateItem? UpdateItem { get; set; }


    protected override Task
    OnInitializedAsync()
    {
        UpdateItem = new()
        {
            DataNonce = ItemToBeEdited.DataNonce,
            Description = ItemToBeEdited.Description,
            EncryptedKeyNonce = ItemToBeEdited.EncryptedKeyNonce,
            EncryptedSymmetricKey = ItemToBeEdited.EncryptedKey,
            Name = ItemToBeEdited.Name,
            Price = ItemToBeEdited.Price,
            SigningPublicKey = ItemToBeEdited.SigningPublicKey,
            SignatureOfKey = ItemToBeEdited.SignatureOfKey
        };

        if (!String.IsNullOrEmpty(ItemToBeEdited.Picture))
        {
            UpdateItem.Picture = ItemToBeEdited.Picture;
            imagePreview = UpdateItem.Picture;
        }
        else
        {
            UpdateItem.Picture = ItemToBeEdited.PictureDecrypted;
            imagePreview = UpdateItem.Picture;
        }


        return base.OnInitializedAsync();
    }

    private async Task
    ValidateForm()
    {
        var validationsPassed = UpdateItemForm!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(UpdateItemForm.EditContext.GetValidationMessages());
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

        if (ItemToBeEdited.IsPublic)
            await NormalUpdate(this.UpdateItem!);
        else
            await UpdateEncryptedItem();

        _processing = false;
        MudDialog.Close();

        await InvokeAsync(StateHasChanged);
    }

    private async Task
    UpdateEncryptedItem()
    {
        var encryptedItem = await JsRuntime.InvokeAsync<ClientDto_CreateItemWithAllKeys>("updateItem",
                UpdateItem,
                ItemToBeEdited.EncryptedKey,
                ItemToBeEdited.DataNonce,
                ItemToBeEdited.ItemId,
                ItemToBeEdited.OwnerId);

        if (encryptedItem != null)
        {
            var transformedShopData = new ClientDto_UpdateItem()
            {
                DataNonce = encryptedItem.DataNonce!,
                Description = encryptedItem.Description,
                EncryptedKeyNonce = encryptedItem.EncryptedKeyNonce!,
                EncryptedSymmetricKey = encryptedItem.EncryptedKey!,
                Name = encryptedItem.Name,
                Picture = encryptedItem.Picture,
                EncryptedPrice = encryptedItem.EncryptedPrice,
                SignatureOfKey = encryptedItem.SignatureOfKey!,
                SigningPublicKey = encryptedItem.SigningPublicKey!,
            };

            await NormalUpdate(transformedShopData);
        }

    }

    private async Task
    NormalUpdate
    (
        ClientDto_UpdateItem UpdateItem
    )
    {
        if (ItemToBeEdited is not null)
        {
            var updateDataResult = await ItemService.EditItem(ItemToBeEdited.ItemId, UpdateItem!);

            if (!updateDataResult.Succsess)
            {
                _snackbar.Add(updateDataResult.Message, Severity.Warning, config => { config.CloseAfterNavigation = true; });

                if (updateDataResult.Errors is not null)
                    ShowValidationsMessages(updateDataResult.Errors);
            }
            else
            {
                _snackbar.Add(updateDataResult.Message, Severity.Success, config => { config.CloseAfterNavigation = true; });
                updateDataResult.Value!.Name = this.UpdateItem!.Name;
                updateDataResult.Value.Description = this.UpdateItem.Description;
                updateDataResult.Value.Price = this.UpdateItem.Price;
                updateDataResult.Value.Picture = this.UpdateItem.Picture;
                await OnItemUpdated.InvokeAsync(updateDataResult.Value);
            }
        }
        else
        {
            _snackbar.Add("Item to be updated is null.", Severity.Success, config => { config.CloseAfterNavigation = true; });
        }
    }

    private async Task
    UploadFile
    (
        IBrowserFile file
    )
    {
        if (file is not null)
        {
            ImageFile = file;
            UpdateItem.Picture = await ConvertToBase64StringAsync(file);
            imagePreview = await ConvertToBase64StringAsync(file);
        }

        StateHasChanged();
    }

    private static async Task<string>
    ConvertToBase64StringAsync
    (
        IBrowserFile file
    )
    {
        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        return Convert.ToBase64String(fileBytes);
    }
}