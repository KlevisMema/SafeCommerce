using SafeCommerce.DataTransormObject.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_UpdateItem
{
    [NoXss]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [NoXss]
    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 100000)]    
    public decimal Price { get; set; }
    public string? Picture { get; set; }

    public string EncryptedSymmetricKey { get; set; } = string.Empty;
    public string DataNonce { get; set; } = string.Empty;
    public string EncryptedKeyNonce { get; set; } = string.Empty;
    public string SignatureOfKey { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}