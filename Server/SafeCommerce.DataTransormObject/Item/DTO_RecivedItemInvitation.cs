using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_RecivedItemInvitation
{
    public Guid ItemId { get; set; }
    public Guid InvitationId { get; set; }
    public Guid InvitingUserId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public InvitationStatus InvitationStatus { get; set; }
    public string InvitingUserName { get; set; } = string.Empty;
}