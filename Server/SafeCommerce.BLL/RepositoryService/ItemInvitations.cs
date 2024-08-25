using AutoMapper;
using SafeCommerce.Utilities.IP;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using SafeCommerce.Utilities.Enums;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeShare.DataAccessLayer.Models;
using SafeCommerce.DataAccessLayer.Models;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.BLL.RepositoryService;

public class ItemInvitations
(
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<ItemInvitations> logger,
    IHttpContextAccessor httpContextAccessor
) : Util_BaseContextDependencies<ApplicationDbContext, ItemInvitations>(
    db,
    mapper,
    logger,
    httpContextAccessor
), IItemInvitations
{
    public async Task<Util_GenericResponse<List<DTO_RecivedItemInvitation>>>
    GetRecivedItemsInvitations
    (
        Guid userId
    )
    {
        try
        {
            var invitations = await _db.ItemInvitations.Include(x => x.Item)
                                                        .Include(x => x.InvitingUser)
                                                        .Include(x => x.InvitedUser)
                                                        .Where(x => x.InvitedUserId == userId.ToString() && x.InvitationStatus == InvitationStatus.Pending)
                                                        .GroupBy(x => x.ItemId)
                                                        .Select(item => item.First())
                                                        .ToListAsync();

            return Util_GenericResponse<List<DTO_RecivedItemInvitation>>.Response
            (
                _mapper.Map<List<DTO_RecivedItemInvitation>>(invitations),
                true,
                "Invitations retrieved successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<List<DTO_RecivedItemInvitation>, ItemInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                Something went wrong, user with [ID] {userId} tried to get all the received invitations.
                """,
                null,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<List<DTO_SentItemInvitation>>>
    GetSentItemInvitations
    (
        Guid userId
    )
    {
        try
        {
            var sentItemInvitations = await _db.ItemInvitations.Include(x => x.Item)
                                                                 .Include(x => x.InvitedUser)
                                                                 .Where
                                                                 (
                                                                    inv => inv.InvitingUserId == userId.ToString() &&
                                                                    inv.InvitationStatus == InvitationStatus.Pending
                                                                 )
                                                                 .ToListAsync();

            return Util_GenericResponse<List<DTO_SentItemInvitation>>.Response
            (
                _mapper.Map<List<DTO_SentItemInvitation>>(sentItemInvitations),
                true,
                "Sent invitations received successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<List<DTO_SentItemInvitation>, ItemInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                Something went wrong, user with [ID] {userId} tried to get all the received invitations.
                """,
                null,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>>
    SendInvitation
    (
        DTO_SendItemInvitationRequest sendInvitation
    )
    {
        try
        {
            var checkPassed = await GenerealChecks(sendInvitation.ItemId, sendInvitation.InvitingUserId, sendInvitation.InvitedUserId, false);

            if (!checkPassed)
            {
                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "Something went wrong!",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var invitation = new ItemInvitation
            {
                InvitingUserId = sendInvitation.InvitingUserId.ToString(),
                InvitedUserId = sendInvitation.InvitedUserId.ToString(),
                ItemId = sendInvitation.ItemId,
                InvitationStatus = InvitationStatus.Pending,
                CreatedAt = DateTime.Now,
                EncryptedKey = sendInvitation.EncryptedKey,
                EncryptedKeyNonce = sendInvitation.EncryptedKeyNonce,
            };

            await _db.ItemInvitations.AddAsync(invitation);
            await _db.SaveChangesAsync();

            _logger.Log
            (
                LogLevel.Information,
                """
                    [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} for the item with id {itemId}.
                    More details : {@invitationDto}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                sendInvitation.InvitingUserId,
                sendInvitation.InvitedUserId,
                sendInvitation.ItemId,
                sendInvitation
            );

            return Util_GenericResponse<bool>.Response
            (
                true,
                true,
                "Invitation sent successfully",
                null,
                System.Net.HttpStatusCode.OK
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong,
                    user with [ID] {sendInvitation.InvitingUserId} tried to send an invite to user with
                    [ID] {sendInvitation.InvitedUserId} for the item with [ID] {sendInvitation.ItemId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>>
    AcceptInvitation
    (
        DTO_InvitationItemRequestActions accepInvitation
    )
    {
        try
        {
            var checkPassed = await GenerealChecks(accepInvitation.ItemId, accepInvitation.InvitingUserId, accepInvitation.InvitedUserId);

            if (!checkPassed)
            {
                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "Something went wrong!",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var invitation = await _db.ItemInvitations.FirstOrDefaultAsync(x => x.Id == accepInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == accepInvitation.InvitingUserId.ToString() &&
                invitation.ItemId == accepInvitation.ItemId &&
                invitation.InvitedUserId == accepInvitation.InvitedUserId.ToString() &&
                invitation.InvitationStatus == InvitationStatus.Pending
            )
            {
                invitation.InvitationStatus = InvitationStatus.Accepted;
                await _db.SaveChangesAsync();

                var itemMember = new ItemShare
                {
                    CreatedAt = DateTime.Now,
                    ItemId = invitation.ItemId,
                    UserId = invitation.InvitedUserId,
                    EncryptedKeyNonce = invitation.EncryptedKeyNonce,
                    EncryptedKey = invitation.EncryptedKey,
                };

                await _db.ItemShares.AddAsync(itemMember);
                _db.Remove(invitation);
                await _db.SaveChangesAsync();

                _logger.Log
                (
                    LogLevel.Information,
                    """
                        Request with IP {IP}.
                        User with id {invitedUserId} accepted the invitation with id {invitationId} made by 
                        the user with id {invitingUserId} for the item with id {itemId}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    accepInvitation.InvitedUserId,
                    accepInvitation.InvitationId,
                    accepInvitation.InvitingUserId,
                    accepInvitation.ItemId
                );

                return Util_GenericResponse<bool>.Response
                (
                    true,
                    true,
                    "Invitation accepted successfully",
                    null,
                    System.Net.HttpStatusCode.OK
                );
            }

            _logger.Log
            (
                LogLevel.Error,
                """
                     Request with IP {IP}.
                     Something went wrong User with id {invitedUserId} tried to accept the invitation with id {invitationId} made by 
                     the user with id {invitingUserId} for the item with id {itemId} but the invitation with that 
                     id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                accepInvitation.InvitedUserId,
                accepInvitation.InvitationId,
                accepInvitation.InvitingUserId,
                accepInvitation.ItemId,
                accepInvitation
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "Something went wrong!",
                null,
                System.Net.HttpStatusCode.NotFound
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemInvitations>.ReturnInternalServerError
            (
               ex,
               _logger,
               $"""
                    Something went wrong in,
                    user with [ID] {accepInvitation.InvitedUserId} tried to accept the invitation by user with id {accepInvitation.InvitingUserId} for the 
                    item with id {accepInvitation.ItemId} with the invitation id {accepInvitation.InvitationId}.
                """,
               false,
               _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>>
    RejectInvitation
    (
        DTO_InvitationItemRequestActions rejectInvitation
    )
    {
        try
        {
            var checkPassed = await GenerealChecks(rejectInvitation.ItemId, rejectInvitation.InvitingUserId, rejectInvitation.InvitedUserId);

            if (!checkPassed)
            {
                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "Something went wrong!",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var invitation = await _db.ItemInvitations.FirstOrDefaultAsync(x => x.Id == rejectInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == rejectInvitation.InvitingUserId.ToString() &&
                invitation.ItemId == rejectInvitation.ItemId &&
                invitation.InvitedUserId == rejectInvitation.InvitedUserId.ToString() &&
                invitation.InvitationStatus == InvitationStatus.Pending
            )
            {
                invitation.InvitationStatus = InvitationStatus.Rejected;
                invitation.ModifiedAt = DateTime.UtcNow;
                _db.Remove(invitation);
                await _db.SaveChangesAsync();

                _logger.Log
                (
                    LogLevel.Information,
                    """
                         Request with IP {IP}.
                         User with id {invitedUserId} rejected the invitation with id {invitationId} made by 
                         the user with id {invitingUserId} for the item with id {itemId}. Dto {@DTO}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    rejectInvitation.InvitedUserId,
                    rejectInvitation.InvitationId,
                    rejectInvitation.InvitingUserId,
                    rejectInvitation.ItemId,
                    rejectInvitation
                );

                return Util_GenericResponse<bool>.Response
                (
                   true,
                   true,
                   "Invitation rejected successfully",
                   null,
                   System.Net.HttpStatusCode.OK
                );
            }

            _logger.Log
            (
                LogLevel.Error,
                """
                     Something went wrong.
                     Request with IP {IP}.
                     User with id {invitedUserId} tried to reject the invitation with id {invitationId} made by 
                     the user with id {invitingUserId} for the item with id {itemId} but the invitation with that 
                     id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                rejectInvitation.InvitedUserId,
                rejectInvitation.InvitationId,
                rejectInvitation.InvitingUserId,
                rejectInvitation.ItemId,
                rejectInvitation
            );

            return Util_GenericResponse<bool>.Response
            (
               false,
               false,
               "Something went wrong!",
               null,
               System.Net.HttpStatusCode.NotFound
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [GroupManagment Module]-[GroupManagment_GroupInvitationsRepository class]-[RejectInvitation Method],
                    user with [ID] {rejectInvitation.InvitedUserId} tried to reject the invitation by user with id {rejectInvitation.InvitingUserId} for the 
                    item with id {rejectInvitation.ItemId} with the invitation id {rejectInvitation.InvitationId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>>
    DeleteSentInvitation
    (
        DTO_InvitationItemRequestActions deleteInvitation
    )
    {
        try
        {
            var checkPassed = await GenerealChecks(deleteInvitation.ItemId, deleteInvitation.InvitingUserId, deleteInvitation.InvitedUserId);

            if (!checkPassed)
            {
                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "Something went wrong!",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var invitation = await _db.ItemInvitations.FirstOrDefaultAsync(x => x.Id == deleteInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == deleteInvitation.InvitingUserId.ToString() &&
                invitation.ItemId == deleteInvitation.ItemId &&
                invitation.InvitedUserId == deleteInvitation.InvitedUserId.ToString() &&
                invitation.InvitationStatus == InvitationStatus.Pending || invitation.InvitationStatus == InvitationStatus.Rejected
            )
            {
                _db.Remove(invitation);
                await _db.SaveChangesAsync();

                _logger.Log
                (
                    LogLevel.Information,
                    """
                        Request with IP {IP}.
                        User with id {invitingUserId} deleted the invitation with id {invitationId} made to  
                        the user with id {invitedUserId} for the item with id {ItemId}. Dto {@DTO}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    deleteInvitation.InvitingUserId,
                    deleteInvitation.InvitationId,
                    deleteInvitation.InvitedUserId,
                    deleteInvitation.ItemId,
                    deleteInvitation
                );

                return Util_GenericResponse<bool>.Response
                (
                    true,
                    true,
                    "Invitation deleted successfully",
                    null,
                    System.Net.HttpStatusCode.OK
                );
            }

            _logger.Log
            (
                LogLevel.Error,
                """
                    Something went wrong.
                    Request with IP {IP}.
                    User with id {invitingUserId} tried to delete the invitation with id {invitationId} made to  
                    the user with id {invitedUserId} for the item with id {ItemId} but the invitation with that 
                    id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                deleteInvitation.InvitingUserId,
                deleteInvitation.InvitationId,
                deleteInvitation.InvitedUserId,
                deleteInvitation.ItemId,
                deleteInvitation
            );

            return Util_GenericResponse<bool>.Response
            (
                false,
                false,
                "Something went wrong!",
                null,
                System.Net.HttpStatusCode.NotFound
            );

        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<bool, ItemInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong,
                    user with [ID] {deleteInvitation.InvitingUserId} tried to delete the invitation to the user with id {deleteInvitation.InvitedUserId} for the 
                    item with id {deleteInvitation.ItemId} with the invitation id {deleteInvitation.InvitationId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    private async Task<bool>
    GenerealChecks
    (
        Guid itemId,
        Guid invitingUserId,
        Guid invitedUserId,
        bool checkItem = true
    )
    {
        if (checkItem)
        {
            var item = await _db.Items.FirstOrDefaultAsync(x => x.ItemId == itemId);

            if (item is null)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                    [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} to a non existing or deleted item.
                 """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    invitingUserId,
                    invitedUserId
                );

                return false;
            }

            if (item.MakePublic && !item.IsPublic && !item.IsApproved)
            {
                _logger.Log
                (
                   LogLevel.Error,
                   """
                    [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} to a non approved item.
                 """,
                   await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                   invitingUserId,
                   invitedUserId
                );

                return false;
            }
        }


        var invitingUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == invitingUserId.ToString() && !x.IsDeleted);

        if (invitingUser is null)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [RESULT] : [IP] {IP}. Inviting user with [ID] {ID} doesn't exists.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                invitingUserId
            );

            return false;
        }

        var invitedUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == invitedUserId.ToString() && !x.IsDeleted);

        if (invitedUser is null)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [RESULT] : [IP] {IP} inviting user with [ID] {ID} send an invitation to user with id {invitedUser},
                    who doesn't exists or is deleted.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                invitingUserId,
                invitedUserId
            );

            return false;
        }

        if (String.IsNullOrEmpty(invitedUser.PublicKey) || String.IsNullOrEmpty(invitedUser.Signature) || String.IsNullOrEmpty(invitedUser.SigningPublicKey))
        {
            _logger.Log
           (
               LogLevel.Error,
               """
                    [RESULT] : [IP] {IP} inviting user with [ID] {ID} send an invitation to user with id {invitedUser},
                    who doesn't have cryptohraphic keys.
                 """,
               await Util_GetIpAddres.GetLocation(_httpContextAccessor),
               invitingUserId,
               invitedUserId
           );

            return false;
        }

        return true;
    }
}