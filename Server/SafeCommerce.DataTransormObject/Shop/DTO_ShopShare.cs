namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_ShopShare
{
    public Guid ShopShareId { get; set; }
    public Guid ShopId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string ShopKey { get; set; } = string.Empty;

    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
}