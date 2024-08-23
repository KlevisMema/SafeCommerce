#region Usings
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Shop; 
#endregion

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IShopProxyService
{
    #region Get
    Task<Util_GenericResponse<DTO_Shop>>
    GetShop
    (
        Guid ownerId,
        Guid shopId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<List<DTO_Shop>>>
    GetUserShops
    (
        Guid ownerId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Shop>>>
    GetPublicSharedShops
    (
        Guid ownerId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ShopMembers>>>
    GetMembersOfTheShop
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken
    );
    #endregion

    #region Post
    Task<Util_GenericResponse<DTO_Shop>>
    CreateShop
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_CreateShop createShopDto
    );

    Task<Util_GenericResponse<bool>>
    InviteUserToShop
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_InviteUserToShop inviteUserToShopDto
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>
    GetShopsSubjectForModeration
    (
        Guid moderatorId,
        string userIp,
        string jwtToken
    );

    Task<Util_GenericResponse<bool>>
    MooderateShop
    (
        string userIp,
        string jwtToken,
        Guid moderatorId,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_ModerateShop inviteUserToShopDto
    );
    #endregion

    #region Put
    Task<Util_GenericResponse<DTO_Shop>>
    EditShop
    (
       Guid shopId,
       Guid ownerId,
       string userIp,
       string jwtToken,
       string forgeryToken,
       string aspNetForgeryToken,
       DTO_UpdateShop editShopDto
    );
    #endregion

    #region Delete
    Task<Util_GenericResponse<bool>>
    DeleteShop
    (
        Guid shopId,
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken
    );

    Task<Util_GenericResponse<bool>>
    RemoveUserFromShop
    (
        Guid ownerId,
        string userIp,
        string jwtToken,
        string forgeryToken,
        string aspNetForgeryToken,
        DTO_RemoveUserFromShop dTO_RemoveUserFromShop
    );
    #endregion
}