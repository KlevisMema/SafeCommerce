namespace SafeCommerce.DataTransormObject.Item;

public class DTO_Item
{
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public Guid ShopId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
}