using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_InviteUserToShop
{
    [Required]
    public Guid ShopId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
}