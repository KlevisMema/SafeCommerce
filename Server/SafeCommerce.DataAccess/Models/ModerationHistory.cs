using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccessLayer.Models;

public class ModerationHistory : Base
{
    [Key]
    public Guid ModerationId { get; set; }

    public Guid? ItemId { get; set; }
    public virtual Item? Item { get; set; }
    public Guid? ShopId { get; set; }
    public virtual Shop? Shop { get; set; }

    public string ModeratorId { get; set; } = string.Empty;
    public virtual ApplicationUser? Moderator { get; set; }

    public bool Approved { get; set; }

    public string Action { get; set; } = string.Empty;
}