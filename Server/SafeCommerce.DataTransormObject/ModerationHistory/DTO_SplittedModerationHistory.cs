namespace SafeCommerce.DataTransormObject.ModerationHistory;

public class DTO_SplittedModerationHistory
{
    public IEnumerable<DTO_ModerationHistory> Shops { get; set; } = [];
    public IEnumerable<DTO_ModerationHistory> Items { get; set; } = [];
}