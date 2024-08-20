using SafeCommerce.Utilities.Enums;

namespace SafeCommerce.DataTransormObject.Invitation;

public class DTO_RecivedInvitations
{
    public Guid InvitingUserId { get; set; }
    public string InvitingUserName { get; set; } = string.Empty;
    public Guid ShopId { get; set; }
    public string ShopName { get; set; } = string.Empty;
    public InvitationStatus InvitationStatus { get; set; }
    public Guid InvitationId { get; set; }
}