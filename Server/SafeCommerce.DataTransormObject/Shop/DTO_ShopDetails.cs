using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_ShopDetails
{
    public Guid ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public List<DTO_Item>? Items { get; set; }
    public List<DTO_User>? SharedUsers { get; set; }
}