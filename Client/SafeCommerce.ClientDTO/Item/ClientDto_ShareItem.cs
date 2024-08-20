namespace SafeCommerce.ClientDTO.Item;
public class ClientDto_ShareItem
{
    public Guid ItemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}