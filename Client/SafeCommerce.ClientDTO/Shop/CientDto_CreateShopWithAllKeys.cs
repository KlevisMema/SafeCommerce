namespace SafeCommerce.ClientDTO.Shop;

public class CientDto_CreateShopWithAllKeys
{
    public Guid ShopId { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EncryptedSymmetricKey { get; set; } = string.Empty;
    public string EncryptedKeyNonce { get; set; } = string.Empty;
    public string DataNonce { get; set; } = string.Empty;
    public string SignatureOfKey { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
    public string SigningPrivateKey { get; set; } = string.Empty;
}