using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.ClientDTO.Invitation;

public class ClientDto_SentInvitations
{
    public string User { get; set; } = string.Empty;
    public string ShopName { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? DataNonce { get; set; }
    public DateTime InvitationTimeSend { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
    public Guid InvitationId { get; set; }
    public Guid InvitedUserId { get; set; }
    public Guid ShopId { get; set; }
}
