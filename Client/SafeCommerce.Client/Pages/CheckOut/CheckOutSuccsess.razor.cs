using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Pages.CheckOut;

public partial class CheckOutSuccsess
{
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;

    private void RedirectToRequestAccountConfirmation()
    {
        _navigationManager?.NavigateTo("/Dashboard");
    }
}