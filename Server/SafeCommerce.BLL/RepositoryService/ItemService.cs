using AutoMapper;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeShare.DataAccessLayer.Models;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.DataTransormObject.Moderation;
using Microsoft.AspNetCore.Identity;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.Utilities.Enums;
using SendGrid.Helpers.Mail;

namespace SafeCommerce.BLL.RepositoryService;
public class ItemService
(
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<ItemService> logger,
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
    public async Task<Util_GenericResponse<DTO_Item>> CreateItem
    (
        DTO_CreateItem createItemDto,
        string ownerId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = _mapper.Map<Item>(createItemDto);
            item.OwnerId = ownerId;

            _db.Items.Add(item);
            await _db.SaveChangesAsync(cancellationToken);

            var createdItem = _mapper.Map<DTO_Item>(item);

            _logger.LogInformation(
                """
                [ItemService]-[CreateItem Method] =>
                [RESULT]: Item created successfully. ItemId: {ItemId}, OwnerId: {OwnerId}.
                """,
                createdItem.ItemId, ownerId);

            return Util_GenericResponse<DTO_Item>.Response(createdItem, true, "Item created successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
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

    public async Task<Util_GenericResponse<DTO_Item>> UpdateItem
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

    public async Task<Util_GenericResponse<bool>> DeleteItem
    (
        Guid itemId,
        string ownerId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.Include(x => x.ModerationHistory).FirstOrDefaultAsync(i => i.ItemId == itemId && i.OwnerId == ownerId, cancellationToken);
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
            _db.ModerationHistories.Remove(item.ModerationHistory!);
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

    public async Task<Util_GenericResponse<DTO_Item>> GetItemById
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

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>> GetItemsByShopId
    (
        Guid shopId,
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var items = await _db.Items
                .Where(i => i.ShopId == shopId && (i.OwnerId == userId || i.Shop.ShopShares.Any(ss => ss.UserId == userId)))
                .Include(i => i.Owner)
                .Select(i => _mapper.Map<DTO_Item>(i))
                .ToListAsync(cancellationToken);

            _logger.LogInformation(
                """
                [ItemService]-[GetItemsByShopId Method] =>
                [RESULT]: Retrieved all items for shop {ShopId} and user {UserId}.
                """,
                shopId, userId);

            return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(items, true, "Items retrieved successfully", null, System.Net.HttpStatusCode.OK);
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

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>> GetItemsSubjectForModeration
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

                return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
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

                return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(null, false, "User is not moderator", null, System.Net.HttpStatusCode.Unauthorized);
            }

            var items = await _db.Items.Where(p => !p.IsPublic && p.MakePublic && !p.IsApproved).Select(i => _mapper.Map<DTO_Item>(i)).ToListAsync(cancellationToken);

            _logger.LogInformation(
                """
                [ItemService]-[GetItemsSubjectForModeration Method] =>
                [RESULT]: Retrieved all items for moderator {modeatorId}.
                """,
                 modeatorId);

            return Util_GenericResponse<IEnumerable<DTO_Item>>.Response(items, true, "Items retrieved successfully", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_Item>, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"""
                Something went wrong in [ItemService]-[GetItemsSubjectForModeration Method], moderator with [ID] {modeatorId} tried to retrieve items for moderation.
                """,
                null,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<bool>> ShareItem
    (
        DTO_ShareItem shareItemDto,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.FindAsync(shareItemDto.ItemId);
            if (item == null || item.OwnerId != shareItemDto.UserId)
            {
                return Util_GenericResponse<bool>.Response(false, false, "Item not found or no permission to share.", null, System.Net.HttpStatusCode.NotFound);
            }

            if (shareItemDto.IsPublic)
            {
                item.IsPublic = true;
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

    public async Task<Util_GenericResponse<bool>> ModerateItem
    (
        DTO_ModerateItem moderateItemDto,
        string moderatorId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var item = await _db.Items.FindAsync(moderateItemDto.ItemId);

            if (item == null)
                return Util_GenericResponse<bool>.Response(false, false, "Item not found.", null, System.Net.HttpStatusCode.NotFound);

            item.IsApproved = moderateItemDto.Approved;
            _db.ModerationHistories.Add(new ModerationHistory
            {
                ItemId = moderateItemDto.ItemId,
                ModeratorId = moderatorId,
                Approved = moderateItemDto.Approved,
                CreatedAt = DateTime.UtcNow,
                ShopId = null
            });

            await _db.SaveChangesAsync(cancellationToken);

            return Util_GenericResponse<bool>.Response(true, true, "Item moderated successfully.", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemService>.ReturnInternalServerError(
                ex,
                _logger,
                $"Error moderating item {moderateItemDto.ItemId} by moderator {moderatorId}.",
                false,
                _httpContextAccessor);
        }
    }

    public async Task<Util_GenericResponse<IEnumerable<DTO_Item>>> GetUserItems
    (
        string userId,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var items = await _db.Items
                .Where(i => i.OwnerId == userId || i.Shop.ShopShares.Any(ss => ss.UserId == userId))
                .Include(i => i.Owner)
                .Include(i => i.Shop)
                .ToListAsync(cancellationToken);

            var itemDtos = _mapper.Map<List<DTO_Item>>(items);

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
}