﻿using SafeCommerce.ClientDTO.Item;
using SafeCommerce.ClientDTO.Invitation;
using SafeCommerce.ClientUtilities.Responses;

namespace SafeCommerce.ClientServices.Interfaces;

public interface IClientService_Invitation
{
    Task<ClientUtil_ApiResponse<bool>>
    AcceptInvitation
    (
        ClientDto_InvitationRequestActions acceptInvitationRequest
    );

    Task<ClientUtil_ApiResponse<bool>>
    DeleteInvitation
    (
        ClientDto_InvitationRequestActions deleteInvitationRequest
    );

    Task<ClientUtil_ApiResponse<List<ClientDto_SentInvitations>>>
    GetSentShopInvitations();

    Task<ClientUtil_ApiResponse<List<ClientDto_RecivedInvitations>>>
    GetShopsInvitations();

    Task<ClientUtil_ApiResponse<bool>>
    RejectInvitation
    (
        ClientDto_InvitationRequestActions rejectInvitationRequest
    );

    Task<ClientUtil_ApiResponse<bool>>
    SendInvitation
    (
        ClientDto_SendInvitationRequest dTO_SendInvitation
    );

    Task<ClientUtil_ApiResponse<List<ClientDto_RecivedItemInvitations>>>
    GetItemsInvitations();

    Task<ClientUtil_ApiResponse<List<ClientDto_SentItemInvitations>>>
    GetSentItemInvitations();

    Task<ClientUtil_ApiResponse<bool>>
    SendItemInvitation
    (
        ClientDto_SendItemInvitationRequest dTO_SendInvitation
    );

    Task<ClientUtil_ApiResponse<bool>>
    AcceptItemInvitation
    (
        ClientDto_InvitationItemRequestActions acceptInvitationRequest
    );

    Task<ClientUtil_ApiResponse<bool>>
    RejectItemInvitation
    (
        ClientDto_InvitationItemRequestActions rejectInvitationRequest
    );

    Task<ClientUtil_ApiResponse<bool>>
    DeleteItemInvitation
    (
        ClientDto_InvitationItemRequestActions deleteInvitationRequest
    );
}