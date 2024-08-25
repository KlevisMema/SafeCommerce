using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_SendItemInvitationRequest
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