namespace SafeCommerce.ClientDTO.Item;

public class ClientDto_GroupedItems
{
    public List<ClientDto_Item> MyPublicItemsSharedWithEveryone { get; set; } = [];
    public List<ClientDto_Item> MyPrivateItemsSharedWithPrivateShop { get; set; } = [];
    public List<ClientDto_Item> MyPublicItemsSharedWithPublicShop { get; set; } = [];
    public List<ClientDto_Item> MyPrivateItemsSharedWithSpecificUsers { get; set; } = [];
    public List<ClientDto_Item> PrivateItemsSharedWithMe { get; set; } = [];
}