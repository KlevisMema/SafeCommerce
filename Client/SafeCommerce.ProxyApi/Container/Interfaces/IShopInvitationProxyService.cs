using SafeCommerce.Utilities.Responses;
using SafeCommerce.DataTransormObject.Invitation;

namespace SafeCommerce.ProxyApi.Container.Interfaces;

public interface IShopInvitationProxyService
{
    Task<Util_GenericResponse<List<DTO_RecivedInvitations>>> 
    GetShopsInvitations
    (
        string userId, 
        string userIp, 
        string jwtToken
    );

    Task<Util_GenericResponse<List<DTO_SentInvitations>>> 
    GetSentShopInvitations
    (
        string userId, 
        string userIp, 
        string jwtToken
    );

    Task<Util_GenericResponse<bool>>
    SendInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_SendInvitationRequest dTO_SendInvitation
    );

    Task<Util_GenericResponse<bool>> 
    AcceptInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions acceptInvitationRequest
    );

    Task<Util_GenericResponse<bool>> 
    RejectInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions rejectInvitationRequest
    );

    Task<Util_GenericResponse<bool>> 
    DeleteInvitation
    (
        string userId, 
        string userIp, 
        string jwtToken, 
        string fogeryToken, 
        string aspNetForgeryToken, 
        DTO_InvitationRequestActions deleteInvitationRequest
    );
}