namespace SafeCommerce.DataTransormObject.ModerationHistory;

public class DTO_ModerationHistory
{
    public Guid ModerationId { get; set; }

    public Guid? ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public Guid? ShopId { get; set; }
    public string ShopName { get; set; } = string.Empty;

    public string ModeratorId { get; set; } = string.Empty;
    public string ModeratorName { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }

    public bool Approved { get; set; }
}