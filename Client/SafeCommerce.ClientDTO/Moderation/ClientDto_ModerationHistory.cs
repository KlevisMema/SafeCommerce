namespace SafeCommerce.ClientDTO.Moderation;

public class ClientDto_ModerationHistory
{
    public Guid ItemId { get; set; }
    public Guid ModeratorId { get; set; }
    public bool Approved { get; set; }
    public DateTime Timestamp { get; set; }
}