using AutoMapper;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using SafeCommerce.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.BLL.RepositoryService;

public class ModerationService
(
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<ModerationService> logger,
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager
) : Util_BaseContextDependencies<ApplicationDbContext, ModerationService>(
    db,
    mapper,
    logger,
    httpContextAccessor
), IModerationService
{
    public async Task<Util_GenericResponse<DTO_SplittedModerationHistory>>
    GetModerationHistoryForModerator
    (
        Guid moderatorId,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var user = await userManager.FindByIdAsync(moderatorId.ToString());

            if (user == null)
            {
                _logger.LogError(
                """
                    [ModerationService]-[GetModerationHistoryForModerator Method] =>
                    [RESULT]:  Owner with id {ownerId} does not exists.
                """,
                 moderatorId);

                return Util_GenericResponse<DTO_SplittedModerationHistory>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            var userIsModerator = await userManager.IsInRoleAsync(user, Role.Moderator.ToString());

            if (!userIsModerator)
            {
                _logger.LogError(
               """
                    [ModerationService]-[GetModerationHistoryForModerator Method] =>
                    [RESULT]:  User with id {userId} does not have access to this.
                """,
                moderatorId);

                return Util_GenericResponse<DTO_SplittedModerationHistory>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.Unauthorized);
            }

            var moderationHistory = await _db.ModerationHistories.Include(m => m.Moderator)
                                                                 .Include(sh => sh.Shop)
                                                                 .Include(i => i.Item)
                                                                 .Where(x => x.ModeratorId == moderatorId.ToString())
                                                                 .Select(rec => _mapper.Map<DTO_ModerationHistory>(rec))
                                                                 .ToListAsync(cancellationToken);

            var result = new DTO_SplittedModerationHistory
            {
                Items = moderationHistory.Where(shop => shop.ShopId == null),
                Shops = moderationHistory.Where(item => item.ItemId == null)
            };

            return Util_GenericResponse<DTO_SplittedModerationHistory>.Response(result, true, "User does not exists", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<DTO_SplittedModerationHistory, ModerationService>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                 Something went wrong, moderator with [ID] {moderatorId} tried to get his moderation history.
                """,
                null,
                _httpContextAccessor
            );
        }
    }
}