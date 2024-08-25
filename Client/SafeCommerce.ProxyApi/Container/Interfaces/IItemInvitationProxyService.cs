using SafeCommerce.DataTransormObject.Item;
using SafeCommerce.Utilities.Responses;

namespace SafeCommerce.ProxyApi.Container.Interfaces
{
    public interface IItemInvitationProxyService
    {
        Task<Util_GenericResponse<bool>> AcceptItemInvitation(string userId, string userIp, string jwtToken, string fogeryToken, string aspNetForgeryToken, DTO_InvitationItemRequestActions acceptInvitationRequest);
        Task<Util_GenericResponse<bool>> DeleteItemInvitation(string userId, string userIp, string jwtToken, string fogeryToken, string aspNetForgeryToken, DTO_InvitationItemRequestActions deleteInvitationRequest);
        Task<Util_GenericResponse<List<DTO_RecivedItemInvitation>>> GetItemsInvitations(string userId, string userIp, string jwtToken);
        Task<Util_GenericResponse<List<DTO_SentItemInvitation>>> GetSentItemsInvitations(string userId, string userIp, string jwtToken);
        Task<Util_GenericResponse<bool>> RejectItemInvitation(string userId, string userIp, string jwtToken, string fogeryToken, string aspNetForgeryToken, DTO_InvitationItemRequestActions rejectInvitationRequest);
        Task<Util_GenericResponse<bool>> SendItemInvitation(string userId, string userIp, string jwtToken, string fogeryToken, string aspNetForgeryToken, DTO_SendItemInvitationRequest dTO_SendInvitation);
    }
}