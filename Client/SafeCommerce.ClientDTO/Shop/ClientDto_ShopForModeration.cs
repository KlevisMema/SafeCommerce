namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_ShopForModeration
{
    public Guid ShopId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}