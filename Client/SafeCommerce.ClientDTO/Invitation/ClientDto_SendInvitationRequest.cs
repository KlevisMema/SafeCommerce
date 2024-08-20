using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Invitation;

public class ClientDto_SendInvitationRequest
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