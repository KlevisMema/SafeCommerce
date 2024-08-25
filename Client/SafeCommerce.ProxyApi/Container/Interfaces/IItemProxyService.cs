#region Usings
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.DataTransormObject.Moderation; 
#endregion

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IItemProxyService
{
    #region Get
    Task<Util_GenericResponse<DTO_Item>>
    GetItemDetails
    (
       Guid itemId,
       Guid ownerId,
       string userIp,
       string jwtToken
    );

    Task<Util_GenericResponse<List<DTO_Item>>>
    GetItemsByShopId
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Item>>>
    GetUserItems
    (
        Guid ownerId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>>
    GetItemsSubjectForModeration
    (
        Guid moderatorId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_PublicItem>>>
    GetPublicSharedItems
    (
        Guid userId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ItemMembers>>>
    GetMembersOfTheItem
    (
        Guid itemId,
        Guid ownerId,
        string userIp,
        string jwtToken
    );
    #endregion

    #region Post
    Task<Util_GenericResponse<DTO_Item>>
    CreateItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_CreateItem createItemDto
    );

    Task<Util_GenericResponse<bool>>
    ShareItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        DTO_ShareItem shareItemDto
    );

    Task<Util_GenericResponse<bool>>
    ModerateItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        DTO_ModerateItem moderateItemDto
    );
    #endregion

    #region Put
    Task<Util_GenericResponse<DTO_Item>>
    EditItem
    (
        Guid itemId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_UpdateItem editItemDto
    );
    #endregion

    #region Delete
    Task<Util_GenericResponse<bool>>
    DeleteItem
    (
        Guid itemId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken
    );

    Task<Util_GenericResponse<bool>>
    RemoveUserFromItem
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_RemoveUserFromItem dTO_RemoveUserFromItem
    );
    #endregion
}