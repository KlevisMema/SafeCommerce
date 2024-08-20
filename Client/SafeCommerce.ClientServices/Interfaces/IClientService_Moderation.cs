using SafeCommerce.ClientDTO.Moderation;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_Moderation
{
    Task<ClientUtil_ApiResponse<ClientDto_SplittedModerationHistory>> 
    GetModerationHistoryForModerator();
}