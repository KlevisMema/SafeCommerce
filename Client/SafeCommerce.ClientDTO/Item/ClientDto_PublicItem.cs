namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_PublicItem
{
    public Guid ItemId { get; set; }
    public decimal? Price { get; set; }
    public string? Picture { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}