using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Pages.Account;

public partial class Profile
{
    private bool DataRetrieved { get; set; } = false;
    [Inject] private IClientService_UserManagment _userManagmentService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private ClientDto_UserInfo? UserInfo { get; set; }

    protected override async Task OnInitializedAsync()
    {
        DataRetrieved = false;

        var getUserInfo = await _userManagmentService.GetUser();

        if (getUserInfo.Succsess)
            UserInfo = getUserInfo.Value;
        else
            NavigationManager.NavigateTo("/Dashbord");

        DataRetrieved = true;
    }
}