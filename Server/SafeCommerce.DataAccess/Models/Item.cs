using SafeCommerce.DataAccess.BaseModels;
using SafeCommerce.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace SafeShare.DataAccessLayer.Models;
public class Item : Base
{
    [Key]
    public Guid ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }

    public Guid ShopId { get; set; }
    public Shop? Shop { get; set; }

    public string OwnerId { get; set; } = string.Empty;
    public virtual ApplicationUser Owner { get; set; } = null!;

    public bool IsApproved { get; set; }
    public bool IsPublic { get; set; }
    public bool MakePublic { get; set; }

    public Guid ModerationHistoryId { get; set; }
    public virtual ModerationHistory? ModerationHistory { get; set; }

    public virtual ICollection<ItemShare>? ItemShares { get; set; }
    public virtual ICollection<Metadata>? Metadata { get; set; }
}