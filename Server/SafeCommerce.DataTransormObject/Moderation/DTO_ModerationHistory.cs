namespace SafeCommerce.DataTransormObject.Moderation;

public class DTO_ShopModerationHistory
{
    public Guid ShopId { get; set; }
    public Guid ModeratorId { get; set; }
    public bool Approved { get; set; }
    public DateTime Timestamp { get; set; }
}