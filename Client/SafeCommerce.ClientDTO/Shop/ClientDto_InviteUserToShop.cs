using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_InviteUserToShop
{
    [Required]
    public Guid ShopId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
}