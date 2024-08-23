using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_ShareItem
{
    public Guid ItemId { get; set; }
    public Guid ShopId { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ItemShareOption? ItemShareOption { get; set; }
}