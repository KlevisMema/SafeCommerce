namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_SaveItemInStore
{
    public Guid ItemId { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public string? EncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? SigningPrivateKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? DataNonce { get; set; }
}