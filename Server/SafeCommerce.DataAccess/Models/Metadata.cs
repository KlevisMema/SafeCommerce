using SafeCommerce.DataAccess.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SafeCommerce.DataAccessLayer.Models;

public class Metadata : Base
{
    [Key]
    public Guid MetadataId { get; set; }

    public Guid ItemId { get; set; }
    public virtual Item? Item { get; set; }

    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}