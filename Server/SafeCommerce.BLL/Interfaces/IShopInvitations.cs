using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.BLL.Interfaces;

public interface IShopInvitations
{
    Task<Util_GenericResponse<bool>> AcceptInvitation(DTO_InvitationRequestActions accepInvitation);
    Task<Util_GenericResponse<bool>> DeleteSentInvitation(DTO_InvitationRequestActions deleteInvitation);
    Task<Util_GenericResponse<List<DTO_RecivedInvitations>>> GetRecivedShopsInvitations(Guid userId);
    Task<Util_GenericResponse<List<DTO_SentInvitations>>> GetSentGroupInvitations(Guid userId);
    Task<Util_GenericResponse<bool>> RejectInvitation(DTO_InvitationRequestActions rejectInvitation);
    Task<Util_GenericResponse<bool>> SendInvitation(DTO_SendInvitationRequest sendInvitation);
}