using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.DataTransormObject.Moderation;

namespace SafeCommerce.BLL.Interfaces;
public interface IItemService
{
    Task<Util_GenericResponse<DTO_Item>> CreateItem
    (
        DTO_CreateItem createItemDto, 
        string ownerId, 
        CancellationToken cancellationToken = default
    );
    
    Task<Util_GenericResponse<DTO_Item>> UpdateItem
    (
        Guid itemId, 
        DTO_UpdateItem updateItemDto, 
        string ownerId, 
        CancellationToken cancellationToken = default
    );
    
    Task<Util_GenericResponse<bool>> DeleteItem
    (
        Guid itemId, 
        string ownerId, 
        CancellationToken cancellationToken = default
    );
    
    Task<Util_GenericResponse<DTO_Item>> GetItemById
    (
        Guid itemId, 
        string userId, 
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Item>>> 
    GetItemsByShopId
    (
        Guid shopId, 
        string userId, 
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>> 
    ShareItem
    (
        Guid ownerId,
        DTO_ShareItem shareItemDto, 
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>> 
    ModerateItem
    (
        DTO_ModerateItem moderateItemDto, 
        string moderatorId, 
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Item>>> 
    GetUserItems
    (
        string userId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>> 
    GetItemsSubjectForModeration
    (
        Guid moderatorId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_PublicItem>>>
    GetPublicSharedItems
    (
        string userId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ItemMembers>>>
    GetMembersOfTheItem
    (
        Guid itemId,
        Guid ownerId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>>
    RemoveUserFromIem
    (
        Guid ownerId,
        DTO_RemoveUserFromItem removeUserFromItem,
        CancellationToken cancellationToken = default
    );
}