using SafeCommerce.Utilities.Enums;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SafeShare.DataAccessLayer.Models;

public class ShopInvitation : Base
{
    [Key]
    public Guid Id { get; set; }
  
    [Required]
    public InvitationStatus InvitationStatus { get; set; }
    
    public Guid ShopId { get; set; }
    public virtual Shop Shop { get; set; } = null!;
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }

    public string InvitedUserId { get; set; } = null!;
    public virtual ApplicationUser InvitedUser { get; set; } = null!;
    
    public string InvitingUserId { get; set; } = null!;
    public virtual ApplicationUser InvitingUser { get; set; } = null!;
}