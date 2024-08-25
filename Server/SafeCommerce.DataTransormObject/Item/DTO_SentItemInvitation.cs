using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_SentItemInvitation
{
    public string User { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string? EncryptedKey { get; set; }
    public string? EncryptedKeyNonce { get; set; }
    public string? DataNonce { get; set; }
    public DateTime InvitationTimeSend { get; set; }
    public InvitationStatus InvitationStatus { get; set; }
    public Guid InvitationId { get; set; }
    public Guid InvitedUserId { get; set; }
    public Guid ItemId { get; set; }
}