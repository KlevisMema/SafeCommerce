using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Shop;
public class ClientDto_ModerateShop
{
    [Required]
    public Guid ShopId { get; set; }
    public bool Approved { get; set; }
}
