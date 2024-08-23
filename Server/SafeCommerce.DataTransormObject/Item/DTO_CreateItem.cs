using SafeCommerce.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_CreateItem
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 100000)]
    public decimal? Price { get; set; }
    public string? EncryptedPrice { get; set; }

    public string? Picture { get; set; }
    public Guid? ShopId { get; set; }

    public string? EncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? DataNonce { get; set; }

    public ItemShareOption? ItemShareOption { get; set; }

    public DTO_ShareItem? DTO_ShareItem { get; set; }
    public List<DTO_ShareItem>? ShareItemToPrivateShop { get; set; }
}