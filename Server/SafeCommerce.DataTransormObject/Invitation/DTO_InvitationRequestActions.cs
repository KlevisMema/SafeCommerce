using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Invitation;

public class DTO_InvitationRequestActions
{
    
    [Required]
    public Guid InvitingUserId { get; set; }
   
    [Required]
    public Guid ShopId { get; set; }
   
    [Required]
    public Guid InvitationId { get; set; }
   
    [Required]
    public Guid InvitedUserId { get; set; }
   
    public string ShopName { get; set; } = string.Empty;
    
    public string UserWhoAcceptedTheInvitation { get; set; } = string.Empty;
}