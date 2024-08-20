namespace SafeCommerce.DataTransormObject.Item;

public class DTO_ShareItem
{
    public Guid ItemId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}