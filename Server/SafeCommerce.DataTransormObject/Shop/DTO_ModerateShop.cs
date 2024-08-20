using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Shop;

public class DTO_ModerateShop
{
    [Required]
    public Guid ShopId { get; set; }
    public bool Approved { get; set; }
}