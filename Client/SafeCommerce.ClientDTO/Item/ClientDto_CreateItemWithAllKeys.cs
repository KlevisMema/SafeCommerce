using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_CreateItemWithAllKeys
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string EncryptedPrice { get; set; } = string.Empty;
    public string? Picture { get; set; }

    public Guid ShopId { get; set; }

    public string? DataNonce { get; set; }
    public string? EncryptedKey { get; set; }
    public string? NonEncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? SigningPrivateKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }

    public ItemShareOption ItemShareOption { get; set; }
    public ClientDto_ShareItem? DTO_ShareItem { get; set; }
}