namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_CartItem
{
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;

    public decimal TotalPrice => Price * Quantity;

    public bool CanDecrease => Quantity > 1;
}