using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Pages.Auth;

public partial class ResetPassword
{
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;

    private string? token;
    private string? email;

    //protected override async Task 
    //OnInitializedAsyn
    //(
    //    bool firstRender
    //)
    //{
    //    if (firstRender)
    //    {

    //    }
    //}

    protected override void OnInitialized()
    {
        var uri = new Uri(_navigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);

        token = query["token"];
        email = query["email"];
    }
}