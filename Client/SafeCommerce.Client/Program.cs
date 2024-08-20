#region Usings
using MudBlazor;
using MudBlazor.Services;
using SafeCommerce.Client;
using System.Text.Unicode;
using Blazored.LocalStorage;
using System.Text.Encodings.Web;
using SafeCommerce.Client.Internal;
using Microsoft.AspNetCore.Components.Web;
using SafeCommerce.ClientUtilities.Helpers;
using SafeCommerce.ClientServices.Services;
using SafeCommerce.ClientServices.Interfaces;
using SafeCommerce.ClientServices.Authentication;
using SafeCommerce.ClientServices.Services.UserManagment;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting; 
#endregion

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddHttpClient(ClientUtilHelpers_Statics.HttpClientName, client =>
{
    client.BaseAddress = new Uri("https://localhost:7380/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddHttpMessageHandler<TokenExpiryHandler>();

builder.Services.AddSingleton<AppState>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<TokenExpiryHandler>();
builder.Services.AddScoped<IClientService_Shop, ClientService_Shop>();
builder.Services.AddScoped<IClientService_Item, ClientService_Item>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IClientService_Moderation, ClientService_Moderation>();
builder.Services.AddScoped<IClientService_Invitation, ClientService_Invitation>();
builder.Services.AddScoped<IClientService_UserManagment, ClientService_UserManagment>();
builder.Services.AddScoped<IClientAuthentication_TokenRefreshService, ClientAuthentication_TokenRefreshService>(); 
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: [UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs]));

await builder.Build().RunAsync();