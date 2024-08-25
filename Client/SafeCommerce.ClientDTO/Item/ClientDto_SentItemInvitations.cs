using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_SentItemInvitations
{
    public Guid ItemId { get; set; }
    public bool IsPublic { get; set; }
    public string? DataNonce { get; set; }
    public Guid InvitationId { get; set; }
    public Guid InvitedUserId { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string User { get; set; } = string.Empty;
    public DateTime InvitationTimeSend { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public InvitationStatus InvitationStatus { get; set; }
}