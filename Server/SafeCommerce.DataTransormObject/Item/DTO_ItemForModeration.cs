using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_ItemForModeration
{
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? Picture { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string ShopName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}