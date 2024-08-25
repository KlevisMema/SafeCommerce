using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_ShareItem
{
    public Guid ItemId { get; set; }
    public Guid ShopId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public ItemShareOption? ItemShareOption { get; set; }
}