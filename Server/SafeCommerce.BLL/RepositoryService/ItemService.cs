using AutoMapper;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using SafeCommerce.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataAccessLayer.Models;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.DataTransormObject.Moderation;

namespace SafeCommerce.BLL.RepositoryService;

public class ItemService
(
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<ItemService> logger,
    IItemInvitations ItemInvitations,
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor
) : Util_BaseContextDependencies<ApplicationDbContext, ItemService>
(
    db,
    mapper,
    logger,
    httpContextAccessor
), IItemService
{
    public async Task<Util_GenericResponse<DTO_Item>>
    CreateItem
    (
        DTO_CreateItem createItemDto,
        string ownerId,
        CancellationToken cancellationToken
    )
    {
        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        var item = new Item();
        try
        {
            ApplicationUser? user = await userManager.FindByIdAsync(ownerId);

            if (user is null)
            {
                _logger.LogWarning
                (
                     """
                        [ItemService]-[CreateItem Method] =>
                        [RESULT]: User was not found {userId}.
                    """,
                    ownerId
                );

                return Util_GenericResponse<DTO_Item>.Response(null, false, "User not found", null, System.Net.HttpStatusCode.NotFound);
            }

            string message = "";

            if (createItemDto.ItemShareOption == ItemShareOption.Shop)
            {
                if (createItemDto.ShareItemToUser is not null)
                {
                    _logger.LogCritical
                    (
                       """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Item not expected to be shared with a user only. Object not correct.
                            OwnerId: {OwnerId}.
                        """,
                       ownerId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Something went wrong, item can not be created. Try again later!", null, System.Net.HttpStatusCode.BadRequest);
                }

                Shop? shop = await _db.Shops.Include(u => u.Owner).FirstOrDefaultAsync(sh => sh.ShopId == createItemDto.ShopId, cancellationToken);

                if (shop is null)
                {
                    _logger.LogWarning
                    (
                         """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Shop was not found {shopId}.
                        """,
                        createItemDto.ShopId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Shop not found", null, System.Net.HttpStatusCode.NotFound);
                }

                if (shop.OwnerId != ownerId)
                {
                    _logger.LogWarning
                    (
                         """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: User {ownerId} is not the owner of Shop  {shopId}.
                        """,
                         ownerId,
                        createItemDto.ShopId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "You are not the owner of the shop", null, System.Net.HttpStatusCode.BadRequest);
                }

                if (shop.MakePublic && !shop.IsPublic && !shop.IsApproved)
                {
                    _logger.LogWarning
                    (
                         """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Shop {shopId} is not yet approved, item cant be added.
                        """,
                        createItemDto.ShopId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, " Shop is not yet approved, item cant be added.", null, System.Net.HttpStatusCode.BadRequest);
                }

                if
                (
                    !shop.MakePublic && !shop.IsPublic && !shop.IsApproved &&
                    String.IsNullOrEmpty(createItemDto.EncryptedKey) &&
                    String.IsNullOrEmpty(createItemDto.SignatureOfKey) &&
                    String.IsNullOrEmpty(createItemDto.SigningPublicKey) &&
                    String.IsNullOrEmpty(createItemDto.EncryptedKeyNonce) &&
                    String.IsNullOrEmpty(createItemDto.DataNonce)
                )
                {
                    _logger.LogWarning
                    (
                         """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Shop {shopId} is private but the item does not have the necessary information for the keys. If shop is encrypted the items inside it need to be encrypted also.
                        """,
                        createItemDto.ShopId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Item are not encrypted it can not be added in a private shop!", null, System.Net.HttpStatusCode.BadRequest);
                }

                item = _mapper.Map<Item>(createItemDto);
                item.OwnerId = ownerId;
                item.ShopId = shop.ShopId;
                item.CreatedAt = DateTime.Now;

                if (shop.IsPublic && shop.MakePublic && shop.IsApproved)
                {
                    item.MakePublic = true;
                    item.IsPublic = false;
                    item.IsApproved = false;
                }

                _db.Items.Add(item);

                if (createItemDto.ShareItemToPrivateShop is not null && createItemDto.ShareItemToPrivateShop.Count > 0)
                {
                    List<ItemShare> itemShares = new();

                    foreach (DTO_ShareItem member in createItemDto.ShareItemToPrivateShop)
                    {
                        itemShares.Add(new ItemShare
                        {
                            ItemId = item.ItemId,
                            EncryptedKey = member.EncryptedKey,
                            EncryptedKeyNonce = member.EncryptedKeyNonce,
                            UserId = member.UserId,
                        });
                    }

                    await _db.ItemShares.AddRangeAsync(itemShares);
                }

                message = "Item created successfully and shared with the shop";
            }
            else if (createItemDto.ItemShareOption == ItemShareOption.ToUser)
            {
                if (createItemDto.ShareItemToUser is null)
                {
                    _logger.LogWarning
                   (
                        """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: User information not provided {DTO_ShareItem}.
                             OwnerId: {OwnerId}
                        """,
                       createItemDto.ShareItemToUser,
                       ownerId
                   );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "User information not provided, item can't be shared with the user not specified.", null, System.Net.HttpStatusCode.BadRequest);
                }

                if (createItemDto.ShareItemToPrivateShop is not null)
                {
                    _logger.LogCritical
                    (
                       """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Item not expected to be shared with users of a pivate shop. Object not correct.
                            OwnerId: {OwnerId}.
                        """,
                       ownerId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Something went wrong, item can not be created. Try again later!", null, System.Net.HttpStatusCode.BadRequest);
                }

                var invitedUser = await userManager.FindByIdAsync(createItemDto.ShareItemToUser.InvitedUserId.ToString());

                if (invitedUser is null || invitedUser.IsDeleted || !invitedUser.EmailConfirmed)
                {
                    _logger.LogWarning
                    (
                        """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Invited user does not exists {userId}. OwnerId: {OwnerId}
                        """,
                        createItemDto.ShareItemToUser.InvitedUserId,
                        ownerId
                     );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "User you are trying to invite does not exists.", null, System.Net.HttpStatusCode.NotFound);
                }

                if (String.IsNullOrEmpty(createItemDto.ShareItemToUser.EncryptedKey) || String.IsNullOrEmpty(createItemDto.ShareItemToUser.EncryptedKeyNonce))
                {
                    _logger.LogWarning
                    (
                        """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Invited user keys missing from the client {userId}. OwnerId: {OwnerId}
                        """,
                        createItemDto.ShareItemToUser.InvitedUserId,
                        ownerId
                     );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Something went wrong, try again later.", null, System.Net.HttpStatusCode.BadRequest);
                }

                item = _mapper.Map<Item>(createItemDto);
                item.OwnerId = ownerId;
                item.ShopId = null;
                item.MakePublic = false;
                item.IsPublic = false;
                item.MakePublic = false;
                item.CreatedAt = DateTime.Now;
                _db.Items.Add(item);

                createItemDto.ShareItemToUser.ItemId = item.ItemId;
                var result = await ItemInvitations.SendInvitation(createItemDto.ShareItemToUser);

                if (!result.Succsess)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Util_GenericResponse<DTO_Item>.Response(null, false, result.Message, null, result.StatusCode);
                }

                message = "Item created successfully and invitation has been sent to the user";
            }
            else
            {
                if
                (
                    !String.IsNullOrEmpty(createItemDto.EncryptedKey) ||
                    !String.IsNullOrEmpty(createItemDto.SignatureOfKey) ||
                    !String.IsNullOrEmpty(createItemDto.SigningPublicKey) ||
                    !String.IsNullOrEmpty(createItemDto.EncryptedKeyNonce) ||
                    !String.IsNullOrEmpty(createItemDto.DataNonce)
                )
                {
                    _logger.LogCritical
                    (
                        """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Item is being shared to everyone but it contains cryptographic keys from client.
                            OwnerId: {OwnerId}.
                        """,
                        ownerId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Something went wrong, item can not be created. Try again later!", null, System.Net.HttpStatusCode.BadRequest);
                }

                if (createItemDto.ShareItemToUser is not null || createItemDto.ShareItemToPrivateShop is not null)
                {
                    _logger.LogCritical
                    (
                       """
                            [ItemService]-[CreateItem Method] =>
                            [RESULT]: Item not expected to be shares to a speciic user or to a shop if the reason was to be shared with a user only. Object not correct.
                            OwnerId: {OwnerId}.
                        """,
                       ownerId
                    );

                    return Util_GenericResponse<DTO_Item>.Response(null, false, "Something went wrong, item can not be created. Try again later!", null, System.Net.HttpStatusCode.BadRequest);
                }

                item = _mapper.Map<Item>(createItemDto);

                item.OwnerId = ownerId;
                item.MakePublic = true;
                item.IsPublic = false;
                item.IsApproved = false;
                item.ShopId = null;
                item.CreatedAt = DateTime.Now;

                _db.Items.Add(item);

                message = "Item created successfully, and it's subject for moderation";
            }

            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation
            (
              """
                [ItemService]-[CreateItem Method] =>
                [RESULT]: Item created successfully. OwnerId: {OwnerId}.
                """,
              ownerId
            );

            return Util_GenericResponse<DTO_Item>.Response(_mapper.Map<DTO_Item>(item), true, message, null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return await Util_LogsHelper<DTO_Item, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[CreateItem Method], user with [ID] {ownerId} tried to create an item.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<DTO_Item>>
    UpdateItem
    (
        Guid itemId,
        DTO_UpdateItem updateItemDto,
        string ownerId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.FirstOrDefaultAsync(i => i.ItemId == itemId && i.OwnerId == ownerId, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning(
                    """
                    [ItemService]-[UpdateItem Method] =>
                    [RESULT]: Item not found or no permission to update. ItemId: {ItemId}, OwnerId: {OwnerId}.
                    """,
                    itemId, ownerId);

                return Util_GenericResponse<DTO_Item>.Response(null, false, "Item not found or no permission to update.", null, System.Net.HttpStatusCode.NotFound);
            }

            _mapper.Map(updateItemDto, item);

            _db.Entry(item).State = EntityState.Modified;
            await _db.SaveChangesAsync(cancellationToken);

            var updatedItem = _mapper.Map<DTO_Item>(item);

            _logger.LogInformation(
                """
                [ItemService]-[UpdateItem Method] =>
                [RESULT]: Item updated successfully. ItemId: {ItemId}, OwnerId: {OwnerId}.
                """,
                itemId, ownerId);

            return Util_GenericResponse<DTO_Item>.Response(updatedItem, true, "Item updated successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_Item, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[UpdateItem Method], user with [ID] {ownerId} tried to update item with [ID] {itemId}.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<bool>>
    DeleteItem
    (
        Guid itemId,
        string ownerId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.Include(x => x.ModerationHistory)
                .Include(x => x.ItemShares)
                .Include(x => x.ItemInvitations)
                .FirstOrDefaultAsync(i => i.ItemId == itemId && i.OwnerId == ownerId, cancellationToken);
            if (item == null)
            {
                _logger.LogWarning(
                    """
                    [ItemService]-[DeleteItem Method] =>
                    [RESULT]: Item not found or no permission to delete. ItemId: {ItemId}, OwnerId: {OwnerId}.
                    """,
                    itemId, ownerId);

                return Util_GenericResponse<bool>.Response(false, false, "Item not found or no permission to delete.", null, System.Net.HttpStatusCode.NotFound);
            }

            if (item.ModerationHistory is not null)
                _db.ModerationHistories.Remove(item.ModerationHistory);

            if (item.ItemShares is not null)
                _db.ItemShares.RemoveRange(item.ItemShares);

            if (item.ItemInvitations is not null)
                _db.ItemInvitations.RemoveRange(item.ItemInvitations);

            _db.Items.Remove(item);
            await _db.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                """
                [ItemService]-[DeleteItem Method] =>
                [RESULT]: Item deleted successfully. ItemId: {ItemId}, OwnerId: {OwnerId}.
                """,
                itemId, ownerId);

            return Util_GenericResponse<bool>.Response(true, true, "Item deleted successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[DeleteItem Method], user with [ID] {ownerId} tried to delete item with [ID] {itemId}.
                """,
                false,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<DTO_Item>>
    GetItemById
    (
        Guid itemId,
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items
                .Where(i => i.ItemId == itemId && (i.OwnerId == userId || i.Shop.ShopShares.Any(ss => ss.UserId == userId)))
                .Include(i => i.Owner)
                .Select(i => _mapper.Map<DTO_Item>(i))
                .FirstOrDefaultAsync(cancellationToken);

            if (item == null)
            {
                _logger.LogWarning(
                    """
                    [ItemService]-[GetItemById Method] =>
                    [RESULT]: Item not found. ItemId: {ItemId}, UserId: {UserId}.
                    """,
                    itemId, userId);

                return Util_GenericResponse<DTO_Item>.Response(null, false, "Item not found.", null, System.Net.HttpStatusCode.NotFound);
            }

            _logger.LogInformation(
                """
                [ItemService]-[GetItemById Method] =>
                [RESULT]: Retrieved item with id {ItemId} for user {UserId}.
                """,
                itemId, userId);

            return Util_GenericResponse<DTO_Item>.Response(item, true, "Item retrieved successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_Item, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[GetItemById Method], user with [ID] {userId} tried to retrieve item with [ID] {itemId}.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>>
    GetItemsByShopId
    (
        Guid shopId,
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var items = await _db.Items
                .Where(i => i.ShopId == shopId)
                .Include(i => i.Owner)
                .Include(i => i.ItemShares)
                .ToListAsync(cancellationToken);

            List<Item> Items = new();

            foreach (var item in items)
            {
                if (item.MakePublic && !item.IsPublic && !item.IsApproved)
                {
                    continue;
                }
                else if (item.MakePublic && item.IsPublic && !item.IsApproved)
                {
                    continue;
                }
                else if (item.MakePublic && !item.IsPublic && item.IsApproved)
                {
                    continue;
                }
                else
                {
                    if (item.OwnerId == userId)
                    {
                        item.EncryptedKey = item.EncryptedKey;
                        item.EncryptedKeyNonce = item.EncryptedKeyNonce;
                    }
                    else
                    {
                        var itemShare = item.ItemShares?.FirstOrDefault(ss => ss.UserId == userId);
                        if (itemShare != null)
                        {
                            item.EncryptedKey = itemShare.EncryptedKey;
                            item.EncryptedKeyNonce = itemShare.EncryptedKeyNonce;
                        }
                    }
                }

                Items.Add(item);
            }

            var itemDtos = _mapper.Map<List<DTO_Item>>(Items);

            _logger.LogInformation(
                """
                [ItemService]-[GetItemsByShopId Method] =>
                [RESULT]: Retrieved all items for shop {ShopId} and user {UserId}.
                """,
                shopId, userId);

            return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(itemDtos, true, "Items retrieved successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_Item>, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[GetItemsByShopId Method], user with [ID] {userId} tried to retrieve items for shop with [ID] {shopId}.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>>
    GetItemsSubjectForModeration
    (
        Guid modeatorId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == modeatorId.ToString(), cancellationToken);

            if (user == null)
            {
                _logger.LogError(
                """
                [ItemService]-[GetItemsSubjectForModeration Method] =>
                [RESULT]:  Moderator with id {modeatorId} does not exists.
                """,
                 modeatorId);

                return Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            bool isModerator = await userManager.IsInRoleAsync(user, Role.Moderator.ToString());

            if (!isModerator)
            {
                _logger.LogError(
                """
                [ItemService]-[GetItemsSubjectForModeration Method] =>
                [RESULT]:  user with id {modeatorId} tried to access moderator only content.
                """,
                 modeatorId);

                return Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>.Response(null, false, "User is not moderator", null, System.Net.HttpStatusCode.Unauthorized);
            }

            var items = await _db.Items.Include(o => o.Owner)
                                       .Include(sh => sh.Shop)
                                       .Where(p => !p.IsPublic && p.MakePublic && !p.IsApproved).Select(i => _mapper.Map<DTO_ItemForModeration>(i)).ToListAsync(cancellationToken);

            _logger.LogInformation(
                """
                [ItemService]-[GetItemsSubjectForModeration Method] =>
                [RESULT]: Retrieved all items for moderator {modeatorId}.
                """,
                 modeatorId);

            return Util_GenericResponse<IEnumerable<DTO_ItemForModeration>>.Response(items, true, "Items retrieved successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_ItemForModeration>, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[GetItemsSubjectForModeration Method], moderator with [ID] {modeatorId} tried to retrieve items for moderation.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ShareItem
    (
        Guid ownerId,
        DTO_ShareItem shareItemDto,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.FindAsync([shareItemDto.ItemId], cancellationToken);

            if (item == null || item.OwnerId != ownerId.ToString())
                return Util_GenericResponse<bool>.Response(false, false, "Item not found", null, System.Net.HttpStatusCode.NotFound);

            if (shareItemDto.ItemShareOption is null)
                return Util_GenericResponse<bool>.Response(false, false, "Something went wrong, try again.", null, System.Net.HttpStatusCode.BadRequest);

            if (shareItemDto.ItemShareOption == ItemShareOption.Everybody)
            {
                item.MakePublic = true;
                item.IsApproved = false;
                item.IsApproved = false;
            }
            else if (shareItemDto.ItemShareOption == ItemShareOption.Shop)
            {
                var shop = await _db.Shops.FindAsync([shareItemDto.ItemId], cancellationToken);

                if (shop is null)
                    return Util_GenericResponse<bool>.Response(false, false, "Shop was not found for item to share", null, System.Net.HttpStatusCode.NotFound);

                item.ShopId = shop.ShopId;
            }
            else
            {
                _db.ItemShares.Add(new ItemShare
                {
                    ItemId = shareItemDto.ItemId,
                    UserId = shareItemDto.UserId
                });
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Util_GenericResponse<bool>.Response(true, true, "Item shared successfully.", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"Error sharing item {shareItemDto.ItemId} by user {shareItemDto.UserId}.",
                false,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<bool>>
    ModerateItem
    (
        DTO_ModerateItem moderateItemDto,
        string moderatorId,
        CancellationToken cancellationToken
    )
    {
        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(m => m.Id == moderatorId.ToString(), cancellationToken);

            if (user == null)
            {
                _logger.LogError(
                """
                        [ItemService]-[ModerateItem Method] =>
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
                        [ItemService]-[ModerateItem Method] =>
                        [RESULT]:  user with id {userId} tried to access moderator only content.
                    """,
                 moderatorId);

                return Util_GenericResponse<bool>.Response(false, false, "User is not moderator.", null, System.Net.HttpStatusCode.Unauthorized);
            }

            var item = await _db.Items.FindAsync(moderateItemDto.ItemId);

            if (item == null)
                return Util_GenericResponse<bool>.Response(false, false, "Item not found.", null, System.Net.HttpStatusCode.NotFound);

            if (item.IsPublic)
                return Util_GenericResponse<bool>.Response(false, false, "Item is already public.", null, System.Net.HttpStatusCode.BadRequest);

            item.IsApproved = moderateItemDto.Approved;
            item.IsPublic = moderateItemDto.Approved;
            item.MakePublic = moderateItemDto.Approved;

            _db.ModerationHistories.Add(new ModerationHistory
            {
                ItemId = moderateItemDto.ItemId,
                ModeratorId = moderatorId,
                Approved = moderateItemDto.Approved,
                CreatedAt = DateTime.Now,
                ShopId = null
            });

            await _db.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return Util_GenericResponse<bool>.Response(true, true, "Item moderated successfully.", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return await Util_LogsHelper<bool, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"Error moderating item {moderateItemDto.ItemId} by moderator {moderatorId}.",
                false,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>>
    GetUserItems
    (
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var items = await _db.Items
                .Include(i => i.Owner)
                .Include(i => i.Shop)
                .Include(i => i.ItemShares)
                .Where(i => i.OwnerId == userId || i.ItemShares.Any(ss => ss.UserId == userId))
                .ToListAsync(cancellationToken);

            List<Item> Items = new();

            foreach (var item in items)
            {
                if (item.MakePublic && !item.IsPublic && !item.IsApproved)
                    continue;
                else if (item.MakePublic && item.IsPublic && !item.IsApproved)
                    continue;
                else if (item.MakePublic && !item.IsPublic && item.IsApproved)
                    continue;
                else
                {
                    if (item.OwnerId == userId)
                    {
                        item.EncryptedKey = item.EncryptedKey;
                        item.EncryptedKeyNonce = item.EncryptedKeyNonce;
                    }
                    else
                    {
                        var itemShare = item.ItemShares?.FirstOrDefault(ss => ss.UserId == userId);
                        if (itemShare != null)
                        {
                            item.EncryptedKey = itemShare.EncryptedKey;
                            item.EncryptedKeyNonce = itemShare.EncryptedKeyNonce;
                        }
                    }
                }

                Items.Add(item);
            }

            var itemDtos = _mapper.Map<List<DTO_Item>>(Items);

            return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(itemDtos, true, "Items retrieved successfully.", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_Item>, ItemService>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"Error retrieving items for user {userId}.",
                null,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_PublicItem>>>
    GetPublicSharedItems
    (
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var items = await _db.Items.Where(i => i.ShopId == null && i.MakePublic == true && i.IsPublic == true && i.IsApproved == true && i.OwnerId != userId).ToListAsync(cancellationToken);

            var itemDtos = _mapper.Map<List<DTO_PublicItem>>(items);

            return Util_GenericResponse<IEnumerable<DTO_PublicItem>>.Response(itemDtos, true, "Items retrieved successfully.", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_PublicItem>, ItemService>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"Error retrieving public items for user {userId}.",
                null,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_ItemMembers>>>
    GetMembersOfTheItem
    (
        Guid itemId,
        Guid ownerId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            ApplicationUser? owner = await userManager.FindByIdAsync(ownerId.ToString());

            if (owner == null)
            {
                _logger.LogError(
                """
                        [ItemService]-[GetMembersOfTheItem Method] =>
                        [RESULT]:  Owner with id {ownerId} does not exists.
                    """,
                 ownerId);

                return Util_GenericResponse<IEnumerable<DTO_ItemMembers>>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            Item? shop = await _db.Items.Include(x => x.ItemShares).ThenInclude(u => u.User).FirstOrDefaultAsync(sh => sh.ItemId == itemId, cancellationToken);

            if (shop == null)
            {
                _logger.LogError(
                """
                        [ItemService]-[GetMembersOfTheItem Method] =>
                        [RESULT]:  Item with id {itemId} does not exists.
                        """,
              itemId);

                return Util_GenericResponse<IEnumerable<DTO_ItemMembers>>.Response(null, false, "Item does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            IEnumerable<DTO_ItemMembers> shopMembers = shop.ItemShares.Select(sh => new DTO_ItemMembers
            {
                UserId = sh.UserId,
                UserName = sh.User.FullName,
                PublicKey = sh.User.PublicKey,
                Signature = sh.User.Signature,
                SigningPublicKey = sh.User.SigningPublicKey,
            });

            _logger.LogInformation(
                """
                        [ItemService]-[GetMembersOfTheItem Method] =>
                        [RESULT]: Owner {ownerId} retieved all the members of item {itemId}.
                        """,
                ownerId,
              itemId);

            return Util_GenericResponse<IEnumerable<DTO_ItemMembers>>.Response(shopMembers, true, "Users retrieved succsessfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_ItemMembers>, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"Error getting all member of the item with id {itemId} by the owner with id {ownerId}",
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<bool>>
    RemoveUserFromIem
    (
        Guid ownerId,
        DTO_RemoveUserFromItem removeUserFromItem,
        CancellationToken cancellationToken = default
    )
    {
        using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var owner = await userManager.FindByIdAsync(ownerId.ToString());

            if (owner == null)
            {
                _logger.LogError(
                """
                        [ItemService]-[RemoveUserFromIem Method] =>
                        [RESULT]:  Owner with id {ownerId} does not exists.
                    """,
                 ownerId);

                return Util_GenericResponse<bool>.Response(false, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            var userToBeRemoved = await userManager.FindByIdAsync(removeUserFromItem.UserId.ToString());

            if (userToBeRemoved == null)
            {
                _logger.LogError(
               """
                        [ItemService]-[RemoveUserFromIem Method] =>
                        [RESULT]:  User with id {userId} does not exists.
                    """,
                removeUserFromItem.UserId);

                return Util_GenericResponse<bool>.Response(false, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            var item = await _db.Items.Include(x => x.ItemShares).FirstOrDefaultAsync(sh => sh.ItemId == removeUserFromItem.ItemId, cancellationToken);

            if (item == null)
            {
                _logger.LogError(
                """
                        [ItemService]-[RemoveUserFromIem Method] =>
                        [RESULT]:  Item with id {itemId} does not exists.
                        """,
              removeUserFromItem.ItemId);

                return Util_GenericResponse<bool>.Response(false, false, "Item does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            if (ownerId == removeUserFromItem.UserId)
                return Util_GenericResponse<bool>.Response(true, true, "Owner cant be removed", null, System.Net.HttpStatusCode.BadRequest);

            if (!item.ItemShares!.Any(x => x.UserId == removeUserFromItem.UserId.ToString()))
            {

                _logger.LogError(
                """
                        [ItemService]-[RemoveUserFromIem Method] =>
                        [RESULT]: User with {userId} does not exists in item with id {itemId}.
                    """,
                removeUserFromItem.UserId,
                removeUserFromItem.ItemId);

                return Util_GenericResponse<bool>.Response(false, false, "Item does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            var shopShareDetach = item.ItemShares!.FirstOrDefault(x => x.ItemId == removeUserFromItem.ItemId && x.UserId == removeUserFromItem.UserId.ToString());

            if (shopShareDetach is not null)
                _db.ItemShares.Remove(shopShareDetach);

            await _db.SaveChangesAsync();

            await transaction.CommitAsync(cancellationToken);

            return Util_GenericResponse<bool>.Response(true, true, "User removed succsessfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);

            return await Util_LogsHelper<bool, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"Error removing user with id {removeUserFromItem.UserId} from item {removeUserFromItem.ItemId} by owner {ownerId}.",
                false,
                _httpContextAccessor);
        }
    }
}