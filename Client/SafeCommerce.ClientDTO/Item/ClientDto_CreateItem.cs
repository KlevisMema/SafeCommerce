using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_CreateItem
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(400)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, 100000)]
    public decimal Price { get; set; }
    public byte[]? Picture { get; set; }
    public Guid? ShopId { get; set; }
}