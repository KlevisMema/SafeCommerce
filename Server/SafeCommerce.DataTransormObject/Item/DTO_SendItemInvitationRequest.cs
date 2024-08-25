using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_SendItemInvitationRequest
{
    [Required]
    public Guid InvitingUserId { get; set; }
    [Required]
    public Guid InvitedUserId { get; set; }
    [Required]
    public Guid ItemId { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
}