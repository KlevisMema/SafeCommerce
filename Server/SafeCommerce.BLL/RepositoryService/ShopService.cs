using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.BLL.Interfaces;
using SafeCommerce.Utilities.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using SafeShare.DataAccessLayer.Models;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.Shop;

namespace SafeCommerce.BLL.RepositoryService
{
    public class ShopService
    (
        ApplicationDbContext db,
        IMapper mapper,
        ILogger<ShopService> logger,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor
    ) : Util_BaseContextDependencies<ApplicationDbContext, ShopService>
    (
        db,
        mapper,
        logger,
        httpContextAccessor
    ), IShopService
    {
        public async Task<Util_GenericResponse<DTO_Shop>>
        CreateShop
        (
            DTO_CreateShop createShopDto,
            string ownerId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shop = _mapper.Map<Shop>(createShopDto);
                shop.OwnerId = ownerId;
                shop.CreatedAt = DateTime.Now;

                _db.Shops.Add(shop);
                await _db.SaveChangesAsync(cancellationToken);

                var createdShop = _mapper.Map<DTO_Shop>(shop);

                _logger.LogInformation(
                    """
                    [ShopService]-[CreateShop Method] =>
                    [RESULT]: Shop created successfully. ShopId: {ShopId}, OwnerId: {OwnerId}.
                    """,
                    createdShop.ShopId, ownerId);

                return Util_GenericResponse<DTO_Shop>.Response(createdShop, true, "Shop created successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<DTO_Shop, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                Something went wrong in [ShopService]-[CreateShop Method], user with [ID] {ownerId} tried to create a shop.
                """,
                    null,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<DTO_Shop>>
        UpdateShop
        (
            Guid shopId,
            DTO_UpdateShop updateShopDto,
            string ownerId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shop = await _db.Shops.FirstOrDefaultAsync(s => s.ShopId == shopId && s.OwnerId == ownerId);
                if (shop == null)
                {
                    _logger.LogWarning(
                        """
                    [ShopService]-[UpdateShop Method] =>
                    [RESULT]: Shop not found or no permission to update. ShopId: {ShopId}, OwnerId: {OwnerId}.
                    """,
                        shopId, ownerId);

                    return Util_GenericResponse<DTO_Shop>.Response(null, false, "Shop not found or no permission to update.", null, HttpStatusCode.NotFound);
                }
                _mapper.Map(updateShopDto, shop);

                shop.ModifiedAt = DateTime.Now;

                _db.Entry(shop).State = EntityState.Modified;
                await _db.SaveChangesAsync(cancellationToken);

                var updatedShop = _mapper.Map<DTO_Shop>(shop);

                _logger.LogInformation(
                    """
                [ShopService]-[UpdateShop Method] =>
                [RESULT]: Shop updated successfully. ShopId: {ShopId}, OwnerId: {OwnerId}.
                """,
                    shopId, ownerId);

                return Util_GenericResponse<DTO_Shop>.Response(updatedShop, true, "Shop updated successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<DTO_Shop, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                Something went wrong in [ShopService]-[UpdateShop Method], user with [ID] {ownerId} tried to update shop with [ID] {shopId}.
                """,
                    null,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<bool>>
        DeleteShop
        (
            Guid shopId,
            string ownerId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shop = await _db.Shops.Include(x => x.ModerationHistory).FirstOrDefaultAsync(s => s.ShopId == shopId && s.OwnerId == ownerId, cancellationToken);
                if (shop == null)
                {
                    _logger.LogWarning(
                        """
                    [ShopService]-[DeleteShop Method] =>
                    [RESULT]: Shop not found or no permission to delete. ShopId: {ShopId}, OwnerId: {OwnerId}.
                    """,
                        shopId, ownerId);

                    return Util_GenericResponse<bool>.Response(false, false, "Shop not found or no permission to delete.", null, HttpStatusCode.NotFound);
                }

                _db.ModerationHistories.Remove(shop.ModerationHistory!);

                _db.Shops.Remove(shop);
                await _db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    """
                [ShopService]-[DeleteShop Method] =>
                [RESULT]: Shop deleted successfully. ShopId: {ShopId}, OwnerId: {OwnerId}.
                """,
                    shopId, ownerId);

                return Util_GenericResponse<bool>.Response(true, true, "Shop deleted successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<bool, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                Something went wrong in [ShopService]-[DeleteShop Method], user with [ID] {ownerId} tried to delete shop with [ID] {shopId}.
                """,
                    false,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<IEnumerable<DTO_Shop>>>
        GetPublicShops
        (
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shops = await _db.Shops
                    .Where(s => s.IsPublic && s.IsApproved && s.MakePublic && s.OwnerId != userId)
                    .Include(s => s.Owner)
                    .Select(s => _mapper.Map<DTO_Shop>(s))
                    .ToListAsync(cancellationToken);

                _logger.LogInformation(
                    """
                        [ShopService]-[GetPublicShops Method] =>
                        [RESULT]: Retrieved all public shops from user {UserId}.
                     """,
                    userId);

                return Util_GenericResponse<IEnumerable<DTO_Shop>>.Response(shops, true, "Shops retrieved successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<IEnumerable<DTO_Shop>, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                        Something went wrong in [ShopService]-[GetPublicShops Method], user with [ID] {userId} tried to retrieve shops.
                    """,
                    null,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<DTO_Shop>>
        GetShopById
        (
            Guid shopId,
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shop = await _db.Shops
                    .Where(s => s.ShopId == shopId && (s.OwnerId == userId || s.ShopShares!.Any(ss => ss.UserId == userId)))
                    .Include(s => s.Owner)
                    .Select(s => _mapper.Map<DTO_Shop>(s))
                    .FirstOrDefaultAsync(cancellationToken);

                if (shop == null)
                {
                    _logger.LogWarning(
                        """
                    [ShopService]-[GetShopById Method] =>
                    [RESULT]: Shop not found. ShopId: {ShopId}, UserId: {UserId}.
                    """,
                        shopId, userId);

                    return Util_GenericResponse<DTO_Shop>.Response(null, false, "Shop not found.", null, HttpStatusCode.NotFound);
                }

                _logger.LogInformation(
                    """
                [ShopService]-[GetShopById Method] =>
                [RESULT]: Retrieved shop with id {ShopId} for user {UserId}.
                """,
                    shopId, userId);

                return Util_GenericResponse<DTO_Shop>.Response(shop, true, "Shop retrieved successfully", null, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<DTO_Shop, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                Something went wrong in [ShopService]-[GetShopById Method], user with [ID] {userId} tried to retrieve shop with [ID] {shopId}.
                """,
                    null,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<bool>>
        InviteUserToShop
        (
            DTO_InviteUserToShop inviteUserToShopDto,
            string ownerId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shop = await _db.Shops.FindAsync(inviteUserToShopDto.ShopId);
                if (shop == null || shop.OwnerId != ownerId)
                {
                    return Util_GenericResponse<bool>.Response(false, false, "Shop not found or no permission to invite.", null, System.Net.HttpStatusCode.NotFound);
                }

                _db.ShopShares.Add(new ShopShare
                {
                    ShopId = inviteUserToShopDto.ShopId,
                    UserId = inviteUserToShopDto.UserId
                });

                await _db.SaveChangesAsync(cancellationToken);

                return Util_GenericResponse<bool>.Response(true, true, "User invited successfully.", null, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<bool, ShopService>.ReturnInternalServerError
                (
                    ex,
                    _logger,
                    $"Error inviting user {inviteUserToShopDto.UserId} to shop {inviteUserToShopDto.ShopId} by owner {ownerId}.",
                    false,
                    _httpContextAccessor
                );
            }
        }

        public async Task<Util_GenericResponse<IEnumerable<DTO_Shop>>>
        GetUserShops
        (
            string userId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var shops = await _db!.Shops!
                                        .Where(s => s.OwnerId == userId || s.ShopShares!.Any(ss => ss.UserId == userId))
                                        .Include(s => s.Owner)
                                        .Include(s => s.Items!)
                                        .ThenInclude(i => i.Metadata)
                                        .Include(s => s.ShopShares!)
                                        .ThenInclude(ss => ss.User)
                                        .ToListAsync(cancellationToken);

                foreach (var shop in shops)
                {
                    if (shop.OwnerId == userId)
                    {
                        shop.EncryptedKey = shop.EncryptedKey;
                        shop.EncryptedKeyNonce = shop.EncryptedKeyNonce;
                    }
                    else
                    {
                        var shopShare = shop.ShopShares?.FirstOrDefault(ss => ss.UserId == userId);
                        if (shopShare != null)
                        {
                            shop.EncryptedKey = shopShare.EncryptedKey;
                            shop.EncryptedKeyNonce = shopShare.EncryptedKeyNonce;
                        }
                    }
                }

                var shopsDto = _mapper.Map<List<DTO_Shop>>(shops);


                return Util_GenericResponse<IEnumerable<DTO_Shop>>.Response(shopsDto, true, "Shops retrieved successfully.", null, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<IEnumerable<DTO_Shop>, ShopService>.ReturnInternalServerError
                (
                    ex,
                    _logger,
                    $"Error retrieving shops for user {userId}.",
                    null,
                    _httpContextAccessor
                );
            }
        }

        public async Task<Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>>
        GetShopsSubjectForModeration
        (
            Guid moderatorId,
            CancellationToken cancellationToken
        )
        {
            try
            {
                ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(m => m.Id == moderatorId.ToString(), cancellationToken);

                if (user == null)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[GetShopsSubjectForModeration Method] =>
                        [RESULT]:  Moderator with id {moderatorId} does not exists.
                    """,
                     moderatorId);

                    return Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                bool isModerator = await userManager.IsInRoleAsync(user, Role.Moderator.ToString());

                if (!isModerator)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[GetShopsSubjectForModeration Method] =>
                        [RESULT]:  user with id {moderatorId} tried to access moderator only content.
                    """,
                     moderatorId);

                    return Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>.Response(null, false, "User is not moderator", null, System.Net.HttpStatusCode.Unauthorized);
                }

                var items = await _db.Shops.Include(x => x.Owner).Where(p => !p.IsPublic && p.MakePublic && !p.IsApproved).Select(i => _mapper.Map<DTO_ShopForModeration>(i)).ToListAsync(cancellationToken);

                _logger.LogInformation(
                    """
                [ShopService]-[GetItemsSubjectForModeration Method] =>
                [RESULT]: Retrieved all items for moderator {moderatorId}.
                """,
                     moderatorId);

                return Util_GenericResponse<IEnumerable<DTO_ShopForModeration>>.Response(items, true, "Items retrieved successfully", null, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<IEnumerable<DTO_ShopForModeration>, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"""
                        Something went wrong in [ShopService]-[GetShopsSubjectForModeration Method], moderator with [ID] {moderatorId} tried to retrieve items for moderation.
                    """,
                    null,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<bool>>
        ModerateShop
        (
            Guid moderatorId,
            DTO_ModerateShop moderateShop,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(m => m.Id == moderatorId.ToString(), cancellationToken);

                if (user == null)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[ModerateShop Method] =>
                        [RESULT]:  Moderator with id {userId} does not exists.
                    """,
                     moderatorId);

                    return Util_GenericResponse<bool>.Response(false, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                bool isModerator = await userManager.IsInRoleAsync(user, Role.Moderator.ToString());

                if (!isModerator)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[ModerateShop Method] =>
                        [RESULT]:  user with id {userId} tried to access moderator only content.
                    """,
                     moderatorId);

                    return Util_GenericResponse<bool>.Response(false, false, "User is not moderator.", null, System.Net.HttpStatusCode.Unauthorized);
                }

                var shop = await _db.Shops.FindAsync(moderateShop.ShopId);

                if (shop == null)
                    return Util_GenericResponse<bool>.Response(false, false, "Shop not found.", null, System.Net.HttpStatusCode.NotFound);

                if (shop.IsPublic)
                    return Util_GenericResponse<bool>.Response(false, false, "Shop is already public.", null, System.Net.HttpStatusCode.BadRequest);

                shop.IsApproved = moderateShop.Approved;
                shop.IsPublic = true;

                _db.ModerationHistories.Add(new ModerationHistory
                {
                    ItemId = null,
                    ModeratorId = moderatorId.ToString(),
                    Approved = moderateShop.Approved,
                    CreatedAt = DateTime.UtcNow,
                    ShopId = moderateShop.ShopId,
                });

                await _db.SaveChangesAsync(cancellationToken);

                return Util_GenericResponse<bool>.Response(true, true, "Shop moderated successfully.", null, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<bool, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"Error moderating shop {moderateShop.ShopId} by moderator {moderatorId}.",
                    false,
                    _httpContextAccessor);
            }
        }

        public async Task<Util_GenericResponse<bool>>
        RemoveUserFromShop
        (
            Guid ownerId,
            DTO_RemoveUserFromShop removeUserFromShop,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                var owner = await userManager.FindByIdAsync(ownerId.ToString());

                if (owner == null)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[RemoveUserFromGroup Method] =>
                        [RESULT]:  Owner with id {ownerId} does not exists.
                    """,
                     ownerId);

                    return Util_GenericResponse<bool>.Response(false, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                var userToBeRemoved = await userManager.FindByIdAsync(removeUserFromShop.UserId.ToString());

                if (userToBeRemoved == null)
                {
                    _logger.LogError(
                   """
                        [ShopService]-[RemoveUserFromGroup Method] =>
                        [RESULT]:  User with id {userId} does not exists.
                    """,
                    removeUserFromShop.UserId);

                    return Util_GenericResponse<bool>.Response(false, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                var shop = await _db.Shops.Include(x => x.ShopShares).FirstOrDefaultAsync(sh => sh.ShopId == removeUserFromShop.ShopId, cancellationToken);

                if (shop == null)
                {
                    _logger.LogError(
                    """
                        [ShopService]-[RemoveUserFromGroup Method] =>
                        [RESULT]:  Shop with id {shopId} does not exists.
                        """,
                  removeUserFromShop.ShopId);

                    return Util_GenericResponse<bool>.Response(false, false, "Shop does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                if (ownerId == removeUserFromShop.UserId)
                    return Util_GenericResponse<bool>.Response(true, true, "Owner cant be removed", null, System.Net.HttpStatusCode.BadRequest);

                if (!shop.ShopShares!.Any(x => x.UserId == removeUserFromShop.UserId.ToString()))
                {

                    _logger.LogError(
                    """
                        ShopService]-[RemoveUserFromGroup Method] =>
                        [RESULT]: user with {userId} does not exists in shop with id {shopId}.
                    """,
                    removeUserFromShop.UserId,
                    removeUserFromShop.ShopId);

                    return Util_GenericResponse<bool>.Response(false, false, "Shop does not exists", null, System.Net.HttpStatusCode.NotFound);
                }

                var shopShareDetach = shop.ShopShares!.FirstOrDefault(x => x.ShopId == removeUserFromShop.ShopId && x.UserId == removeUserFromShop.UserId.ToString());

                _db.ShopShares.Remove(shopShareDetach!);

                await _db.SaveChangesAsync();

                return Util_GenericResponse<bool>.Response(true, true, "User removed succsessfully", null, System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return await Util_LogsHelper<bool, ShopService>.ReturnInternalServerError(
                    ex,
                    _logger,
                    $"Error removing user with id {removeUserFromShop.UserId} from shop {removeUserFromShop.ShopId} by owner {ownerId}.",
                    false,
                    _httpContextAccessor);
            }
        }
    }
}