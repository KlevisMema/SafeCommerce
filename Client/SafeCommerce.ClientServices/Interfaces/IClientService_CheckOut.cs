using SafeCommerce.ClientDTO.Item;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_CheckOut
{
    Task<string>
    CheckOut
    (
        string userId,
        List<ClientDto_CartItem> CartItems
    );
}