using SafeCommerce.DataAccess.BaseModels;
using SafeCommerce.DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccessLayer.Models;
    
public class ItemShare : Base
{
    [Key]
    public Guid ItemShareId { get; set; }

    public Guid ItemId { get; set; }
    public virtual Item? Item { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
}