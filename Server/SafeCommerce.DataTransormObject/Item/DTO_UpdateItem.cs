using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataTransormObject.Item;

public class DTO_UpdateItem
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
    public string? Picture { get; set; }
}