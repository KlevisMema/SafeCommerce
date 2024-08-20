using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_CreateShop
{
    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 8)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(maximumLength: 400, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;
    public bool MakePublic { get; set; }

    public string EncryptedSymmetricKey { get; set; } = string.Empty;
    public string DataNonce { get; set; } = string.Empty;
    public string EncryptedKeyNonce { get; set; } = string.Empty;
    public string SignatureOfKey { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}