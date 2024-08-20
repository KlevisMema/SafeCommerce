using SafeCommerce.DataTransormObject.ModerationHistory;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.ProxyApi.Container.Interfaces
{
    public interface IModerationProxyService
    {
        Task<Util_GenericResponse<DTO_SplittedModerationHistory>> GetModerationHistoryForModerator(Guid moderatorId, string userIp, string jwtToken);
    }
}