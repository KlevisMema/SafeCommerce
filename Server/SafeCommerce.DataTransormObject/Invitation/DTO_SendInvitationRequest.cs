using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Invitation;

public class DTO_SendInvitationRequest
{
    [Required]
    public Guid InvitingUserId { get; set; }
    [Required]
    public Guid InvitedUserId { get; set; }
    [Required]
    public Guid ShopId { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
}