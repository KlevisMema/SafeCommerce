using SafeCommerce.ClientDTO.Shop;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_ItemForModeration
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