using SafeCommerce.ClientDTO.Enums;
using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_CreateItem
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
    public decimal? Price { get; set; }
    public string? EncryptedPrice { get; set; }

    public string? Picture { get; set; }
    public Guid ShopId { get; set; }

    public string? DataNonce { get; set; }
    public string? EncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }

    public ItemShareOption ItemShareOption { get; set; }

    public ClientDto_SendItemInvitationRequest? ShareItemToUser { get; set; }
    public List<ClientDto_ShareItem>? ShareItemToPrivateShop { get; set; }
}