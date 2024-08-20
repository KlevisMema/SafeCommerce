using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SafeShare.DataAccessLayer.Models;

public class ShopShare : Base
{
    [Key]
    public Guid ShopShareId { get; set; }

    public Guid ShopId { get; set; }
    public virtual Shop? Shop { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }

    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }
}