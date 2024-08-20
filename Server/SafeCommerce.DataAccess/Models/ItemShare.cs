using SafeCommerce.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace SafeShare.DataAccessLayer.Models;

public class ItemShare
{
    [Key]
    public Guid ItemShareId { get; set; }

    public Guid ItemId { get; set; }
    public virtual Item? Item { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }
}