using Blazored.LocalStorage;
using SafeCommerce.ClientDTO.Enums;
using Microsoft.AspNetCore.Components;
using SafeCommerce.Client.Internal.Helpers;

namespace SafeCommerce.Client.Shared.Components;

public partial class Dashboard
{
    [Inject] private ILocalStorageService LocalStorageService { get; set; } = null!;
    [Parameter] public bool DataRetrieved { get; set; }

    private Role? UserRole { get; set; }

    protected override async Task OnInitializedAsync()
    {
        this.UserRole = await UserUtilities.GetRole(LocalStorageService);

        if (UserRole == Role.NoRole)
        {
            // DO SMTH
        }
    }
}