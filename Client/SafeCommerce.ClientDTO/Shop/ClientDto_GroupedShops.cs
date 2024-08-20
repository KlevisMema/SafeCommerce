namespace SafeCommerce.ClientDTO.Shop;

public class ClientDto_GroupedShops
{
    public List<ClientDto_Shop> MyPrivateShops { get; set; } = [];
    public List<ClientDto_Shop> MyPublicShops { get; set; } = [];
    public List<ClientDto_Shop> JoinedShops { get; set; } = [];
}