namespace SafeCommerce.ClientDTO.Moderation;

public class ClientDto_SplittedModerationHistory
{
    public IEnumerable<ClientDto_ModerationHistory> Shops { get; set; } = [];
    public IEnumerable<ClientDto_ModerationHistory> Items { get; set; } = [];
}