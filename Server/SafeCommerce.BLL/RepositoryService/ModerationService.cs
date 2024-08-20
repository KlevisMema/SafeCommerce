using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.DataTransormObject.Moderation;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.Utilities.Log;
using SafeCommerce.Utilities.Responses;

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
)
{
    public async Task<Util_GenericResponse<IEnumerable<DTO_ModerationHistory>>>
    GetModerationHistoryForModerator
    (
        Guid moderatorId
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

                return Util_GenericResponse<IEnumerable<DTO_ModerationHistory>>.Response(null, false, "User does not exists", null, System.Net.HttpStatusCode.NotFound);
            }

            return Util_GenericResponse<IEnumerable<DTO_ModerationHistory>>.Response(null, true, "User does not exists", null, System.Net.HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<IEnumerable<DTO_ModerationHistory>, ModerationService>.ReturnInternalServerError
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