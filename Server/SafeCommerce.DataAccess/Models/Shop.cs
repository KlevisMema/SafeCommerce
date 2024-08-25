using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccessLayer.Models;

public class Shop : Base
{
    [Key]
    public Guid ShopId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;
    public string? EncryptedKey { get; set; }
    public string? SignatureOfKey { get; set; }
    public string? SigningPublicKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? DataNonce { get; set; }
    public bool MakePublic { get; set; }
    public bool IsPublic { get; set; }
    public bool IsApproved { get; set; }

    public string OwnerId { get; set; } = string.Empty;
    public virtual ApplicationUser Owner { get; set; } = null!;

    public Guid ModerationHistoryId { get; set; }
    public virtual ModerationHistory? ModerationHistory { get; set; }

    public virtual ICollection<Item>? Items { get; set; }
    public virtual ICollection<ShopShare>? ShopShares { get; set; }
    public virtual ICollection<ShopInvitation>? ShopInvitations { get; set; }
}