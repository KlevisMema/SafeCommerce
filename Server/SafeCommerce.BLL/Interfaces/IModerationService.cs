using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.BLL.Interfaces;

public interface IModerationService
{
    Task<Util_GenericResponse<DTO_SplittedModerationHistory>>
    GetModerationHistoryForModerator
    (
        Guid moderatorId,
        CancellationToken cancellationToken = default
    );
}