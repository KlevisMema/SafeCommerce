using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_CreateShop
{
    [Required]
    [StringLength(maximumLength: 100, MinimumLength = 8)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(maximumLength: 400, MinimumLength = 20)]
    public string Description { get; set; } = string.Empty;
    
    public string? EncryptedSymmetricKey { get; set; }
    public string? DataNonce { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }

    public bool MakePublic { get; set; }
}