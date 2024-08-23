using SafeCommerce.ClientDTO.Item;
using Microsoft.AspNetCore.Components;

namespace SafeCommerce.Client.Shared.Components.Shop;

public partial class ShopPreview
{
    [Parameter] public List<ClientDto_Item>? Items { get; set; }
}