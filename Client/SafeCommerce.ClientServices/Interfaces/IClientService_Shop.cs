using SafeCommerce.ClientDTO.Shop;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_Shop
{
    Task<ClientUtil_ApiResponse<ClientDto_Shop>> 
    CreateShop
    (
        ClientDto_CreateShop createShopDto
    );

    Task<ClientUtil_ApiResponse<bool>> 
    DeleteShop
    (
        Guid shopId
    );

    Task<ClientUtil_ApiResponse<ClientDto_Shop>> 
    EditShop
    (
        Guid shopId, 
        ClientDto_UpdateShop editShopDto
    );

    Task<ClientUtil_ApiResponse<ClientDto_Shop>> 
    GetShop
    (
        Guid shopId
    );

    Task<ClientUtil_ApiResponse<List<ClientDto_Shop>>> 
    GetUserShops();

    Task<ClientUtil_ApiResponse<bool>> 
    InviteUserToShop
    (
        Guid shopId, 
        ClientDto_InviteUserToShop inviteUserToShopDto
    );

    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ShopForModeration>>>
    GetShopsSubjectForModeration();


    Task<ClientUtil_ApiResponse<bool>>
    ModerateShop
    (
       ClientDto_ModerateShop moderateShop
    );

    Task<ClientUtil_ApiResponse<bool>>
    RemoveUserFromShop
    (
        ClientDto_RemoveUserFromShop clientDto_RemoveUserFromShop
    );

    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_Shop>>>
    GetPublicSharedShops();
}