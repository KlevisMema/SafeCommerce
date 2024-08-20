using SafeCommerce.Client.Shared;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Pages;

public partial class Index
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
}