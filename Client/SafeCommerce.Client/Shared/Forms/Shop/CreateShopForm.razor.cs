using MudBlazor;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Shop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SafeCommerce.ClientServices.Interfaces;

namespace SafeCommerce.Client.Shared.Forms.Shop;

public partial class CreateShopForm
{
    [Inject] private ISnackbar _snackbar { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IClientService_Shop ShopService { get; set; } = null!;
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;

    private bool _processing = false;
    private EditForm? CreateShopFormContext;
    private ClientDto_CreateShop CreateShopDto { get; set; } = new();

    private async Task
    ValidateForm()
    {
        var validationsPassed = CreateShopFormContext!.EditContext!.Validate()!;

        if (!validationsPassed)
            ShowValidationsMessages(CreateShopFormContext.EditContext.GetValidationMessages());
        else
            await SubmitCreateShopForm();
    }

    private void
    ShowValidationsMessages
    (
        IEnumerable<string> validationMessages
    )
    {
        foreach (var validationMessage in validationMessages)
        {
            _snackbar.Add(validationMessage, Severity.Warning, config =>
            {
                config.CloseAfterNavigation = true;
                config.VisibleStateDuration = 3000;
            });
        }
    }

    private async Task
    SubmitCreateShopForm()
    {
        _processing = true;
        await Task.Delay(1000);

        if (CreateShopDto.MakePublic)
        {
            var createNonEncryptedShop = await ShopService.CreateShop(CreateShopDto);

            if (createNonEncryptedShop.Succsess)
            {
                _snackbar.Add("Shop is created successfully, and its subject to moderation.", Severity.Success,
                config => { config.CloseAfterNavigation = true; });
            }
        }
        else
        {
            string? userId = await LocalStorageService.GetItemAsStringAsync("Id");

            var encryptedShop = await JsRuntime.InvokeAsync<CientDto_CreateShopWithAllKeys>("encryptShopData", CreateShopDto, userId);

            if (encryptedShop == null)
                return;

            var transformedShopData = new ClientDto_CreateShop()
            {
                DataNonce = encryptedShop.DataNonce,
                Description = encryptedShop.Description,
                EncryptedKeyNonce = encryptedShop.EncryptedKeyNonce,
                EncryptedSymmetricKey = encryptedShop.EncryptedSymmetricKey,
                Name = encryptedShop.Name,
                SignatureOfKey = encryptedShop.SignatureOfKey,
                SigningPublicKey = encryptedShop.SigningPublicKey,
            };

            var createShopResult = await ShopService.CreateShop(transformedShopData);

            if (!createShopResult.Succsess && createShopResult.Value is null)
            {
                _snackbar.Add(createShopResult.Message, Severity.Warning,
                    config => { config.CloseAfterNavigation = true; });

                if (createShopResult.Errors is not null)
                    ShowValidationsMessages(createShopResult.Errors);
            }
            else
            {
                encryptedShop.OwnerId = createShopResult.Value.OwnerId;
                encryptedShop.ShopId = createShopResult.Value.ShopId;

                await JsRuntime.InvokeVoidAsync("saveShopKeysData", encryptedShop);

                _snackbar.Add(createShopResult.Message, Severity.Success,
                    config => { config.CloseAfterNavigation = true; });
            }
        }

        CreateShopDto = new();
        _processing = false;
    }
}