using SafeCommerce.Utilities.Enums;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccessLayer.Models;
using System.ComponentModel.DataAnnotations;
using SafeCommerce.DataAccess.BaseModels;

namespace SafeShare.DataAccessLayer.Models;

public class ItemInvitation : Base
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public InvitationStatus InvitationStatus { get; set; }

    public Guid ItemId { get; set; }
    public virtual Item Item { get; set; } = null!;

    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }

    public string InvitedUserId { get; set; } = null!;
    public virtual ApplicationUser InvitedUser { get; set; } = null!;

    public string InvitingUserId { get; set; } = null!;
    public virtual ApplicationUser InvitingUser { get; set; } = null!;
}