using SafeCommerce.ClientDTO.Validators;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_InviteUserToShop
{
    [Required]
    public Guid ShopId { get; set; }

    [NoXss]
    [Required]
    public string UserId { get; set; } = string.Empty;
}