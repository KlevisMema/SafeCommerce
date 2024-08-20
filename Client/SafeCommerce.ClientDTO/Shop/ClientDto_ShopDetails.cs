using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.User;

namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_ShopDetails
{
    public Guid ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public List<ClientDto_Item>? Items { get; set; }
    public List<ClientDto_User>? SharedUsers { get; set; }
}