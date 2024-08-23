namespace SafeCommerce.DataTransormObject.Item;

public class DTO_Item
{
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? EncryptedPrice { get; set; }
    public string? Picture { get; set; }
    public Guid ShopId { get; set; }

    public string OwnerId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerPublicKey { get; set; } = string.Empty;
    public string OwnerSignature { get; set; } = string.Empty;
    public string OwnerSigningPublicKey { get; set; } = string.Empty;

    public bool IsApproved { get; set; }
    public bool IsPublic { get; set; }
    public bool MakePublic { get; set; }

    public string? DataNonce { get; set; }
    public string? EncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
}