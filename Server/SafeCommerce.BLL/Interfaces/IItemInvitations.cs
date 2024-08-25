using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.BLL.Interfaces;

public interface IItemInvitations
{
    Task<Util_GenericResponse<bool>> 
    AcceptInvitation
    (
        DTO_InvitationItemRequestActions accepInvitation
    );

    Task<Util_GenericResponse<bool>> 
    DeleteSentInvitation
    (
        DTO_InvitationItemRequestActions deleteInvitation
    );

    Task<Util_GenericResponse<List<DTO_RecivedItemInvitation>>> 
    GetRecivedItemsInvitations
    (
        Guid userId
    );

    Task<Util_GenericResponse<List<DTO_SentItemInvitation>>> 
    GetSentItemInvitations
    (
        Guid userId
    );

    Task<Util_GenericResponse<bool>> 
    RejectInvitation
    (
        DTO_InvitationItemRequestActions rejectInvitation
    );

    Task<Util_GenericResponse<bool>> 
    SendInvitation
    (
        DTO_SendItemInvitationRequest sendInvitation
    );
}