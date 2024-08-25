using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Moderation;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_Item
{
    Task<ClientUtil_ApiResponse<ClientDto_Item>> 
    CreateItem
    (
        ClientDto_CreateItem createItemDto
    );

    Task<ClientUtil_ApiResponse<bool>> 
    DeleteItem
    (
        Guid itemId
    );

    Task<ClientUtil_ApiResponse<ClientDto_Item>> 
    EditItem
    (
        Guid itemId, 
        ClientDto_UpdateItem editItemDto
    );

    Task<ClientUtil_ApiResponse<ClientDto_Item>> 
    GetItemDetails
    (
        Guid itemId
    );
    
    Task<ClientUtil_ApiResponse<List<ClientDto_Item>>> 
    GetItemsByShopId
    (
        Guid shopId
    );
    
    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_Item>>> 
    GetUserItems();

    Task<ClientUtil_ApiResponse<bool>> 
    ModerateItem
    (
        ClientDto_ModerateItem moderateItemDto
    );

    Task<ClientUtil_ApiResponse<bool>> 
    ShareItem
    (
        ClientDto_ShareItem shareItemDto
    );

    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemForModeration>>>
    GetItemsSubjectForModeration();


    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_PublicItem>>>
    GetPublicSharedItems();

    Task<ClientUtil_ApiResponse<IEnumerable<ClientDto_ItemMembers>>>
    GetMembersOfTheItem
    (
        Guid itemId
    );

    Task<ClientUtil_ApiResponse<bool>>
    RemoveUserFromItem
    (
        ClientDto_RemoveUserFromItem clientDto_RemoveUserFromItem
    );
}