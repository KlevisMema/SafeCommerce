using SafeCommerce.ClientDTO.Enums;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_RecivedItemInvitations
{
    public Guid ItemId { get; set; }
    public Guid InvitationId { get; set; }
    public Guid InvitingUserId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public InvitationStatus InvitationStatus { get; set; }
    public string InvitingUserName { get; set; } = string.Empty;
}