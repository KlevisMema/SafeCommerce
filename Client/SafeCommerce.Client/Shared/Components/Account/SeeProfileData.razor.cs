using Microsoft.AspNetCore.Components;
using SafeCommerce.ClientDTO.AccountManagment;

namespace SafeCommerce.Client.Shared.Components.Account
{
    public partial class SeeProfileData
    {
        [Parameter] public ClientDto_UserInfo? UserInfo { get; set; }
        [Parameter] public bool DataRetrieved { get; set; }

        protected override void OnInitialized()
        {
            UserInfo ??= new();

            base.OnInitialized();
        }
    }
}
