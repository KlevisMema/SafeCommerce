using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_UpdateItem
{
    [NoXss]
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [NoXss]
    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    [NoXss]
    [Required]
    [Range(0, 100000)]
    public decimal Price { get; set; }
    public string EncryptedPrice { get; set; } = string.Empty;
    public string? Picture { get; set; }

    public string EncryptedSymmetricKey { get; set; } = string.Empty;
    public string DataNonce { get; set; } = string.Empty;
    public string EncryptedKeyNonce { get; set; } = string.Empty;
    public string SignatureOfKey { get; set; } = string.Empty;
    public string SigningPublicKey { get; set; } = string.Empty;
}