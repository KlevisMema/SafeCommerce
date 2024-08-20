using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.BLL.Interfaces;

public interface IShopService
{
    Task<Util_GenericResponse<DTO_Shop>> CreateShop
    (
        DTO_CreateShop createShopDto,
        string ownerId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>> DeleteShop
    (
        Guid shopId,
        string ownerId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<DTO_Shop>> GetShopById
    (
        Guid shopId,
        string userId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Shop>>>
    GetPublicShops
    (
        string userId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<DTO_Shop>> UpdateShop
    (
        Guid shopId,
        DTO_UpdateShop updateShopDto,
        string ownerId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>> InviteUserToShop
    (
        DTO_InviteUserToShop inviteUserToShopDto,
        string ownerId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_Shop>>> GetUserShops
    (
        string userId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>> GetShopsSubjectForModeration
    (
        Guid moderatorId,
        CancellationToken cancellationToken = default
    );

    Task<Util_GenericResponse<bool>>
    ModerateShop
    (
        Guid moderatorId,
        DTO_ModerateShop moderateShop,
        CancellationToken cancellationToken
    );

    Task<Util_GenericResponse<bool>>
    RemoveUserFromShop
    (
        Guid ownerId,
        DTO_RemoveUserFromShop removeUserFromShop,
        CancellationToken cancellationToken = default
    );
}