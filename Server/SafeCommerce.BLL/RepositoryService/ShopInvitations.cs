using AutoMapper;
using SafeCommerce.Utilities.IP;
using Microsoft.AspNetCore.Http;
using SafeCommerce.Utilities.Log;
using SafeCommerce.Utilities.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SafeCommerce.DataAccess.Context;
using SafeCommerce.Utilities.Responses;
using SafeShare.DataAccessLayer.Models;
using SafeCommerce.Utilities.Dependencies;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.BLL.Interfaces;

namespace SafeCommerce.BLL.RepositoryService;

public class ShopInvitations
    (
    ApplicationDbContext db,
    IMapper mapper,
    ILogger<ShopInvitations> logger,
    IHttpContextAccessor httpContextAccessor
) : Util_BaseContextDependencies<ApplicationDbContext, ShopInvitations>(
    db,
    mapper,
    logger,
    httpContextAccessor
), IShopInvitations
{
    public async Task<Util_GenericResponse<List<DTO_RecivedInvitations>>> GetRecivedShopsInvitations(Guid userId)
    {
        try
        {
            var invitations = await _db.ShopInvitations.Include(x => x.Shop)
                                                        .Include(x => x.InvitingUser)
                                                        .Include(x => x.InvitedUser)
                                                        .Where(x => x.InvitedUserId == userId.ToString() && x.InvitationStatus == InvitationStatus.Pending)
                                                        .GroupBy(x => x.ShopId)
                                                        .Select(group => group.First())
                                                        .ToListAsync();

            return Util_GenericResponse<List<DTO_RecivedInvitations>>.Response
            (
                _mapper.Map<List<DTO_RecivedInvitations>>(invitations),
                true,
                "Invitations retrieved successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<List<DTO_RecivedInvitations>, ShopInvitations>.ReturnInternalServerError
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

    public async Task<Util_GenericResponse<List<DTO_SentInvitations>>> GetSentGroupInvitations(Guid userId)
    {
        try
        {
            var sentShopInvitations = await _db.ShopInvitations.Include(x => x.Shop)
                                                                 .Include(x => x.InvitedUser)
                                                                 .Where
                                                                 (
                                                                    inv => inv.InvitingUserId == userId.ToString() &&
                                                                    inv.InvitationStatus == InvitationStatus.Pending
                                                                 )
                                                                 .ToListAsync();

            return Util_GenericResponse<List<DTO_SentInvitations>>.Response
            (
                _mapper.Map<List<DTO_SentInvitations>>(sentShopInvitations),
                true,
                "Sent invitations received successfully",
                null,
                System.Net.HttpStatusCode.OK
            );
        }
        catch (Exception ex)
        {
            return await Util_LogsHelper<List<DTO_SentInvitations>, ShopInvitations>.ReturnInternalServerError
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

    public async Task<Util_GenericResponse<bool>> SendInvitation(DTO_SendInvitationRequest sendInvitation)
    {
        try
        {
            var checkPassed = await GenerealChecks(sendInvitation.ShopId, sendInvitation.InvitingUserId, sendInvitation.InvitedUserId);

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

            var isAdminWhoInvited = await _db.ShopShares.Include(shop => shop.Shop)
                                                        .ThenInclude(user => user.Owner)
                                                        .AnyAsync
                                                        (
                                                            x => x.ShopId == sendInvitation.ShopId &&
                                                            x.UserId == sendInvitation.InvitingUserId.ToString()
                                                            && x.Shop.OwnerId == sendInvitation.InvitingUserId.ToString()
                                                        );

            var invitationIsAlreadySent = await _db.ShopInvitations.Include(x => x.InvitingUser)
                                                                    .Include(x => x.InvitedUser)
                                                                    .AnyAsync(x => x.ShopId == sendInvitation.ShopId &&
                                                                                    x.InvitedUserId == sendInvitation.InvitedUserId.ToString() &&
                                                                                    x.InvitingUserId == sendInvitation.InvitingUserId.ToString() &&
                                                                                    x.InvitationStatus == InvitationStatus.Pending &&
                                                                                    !x.InvitedUser.IsDeleted &&
                                                                                    !x.InvitingUser.IsDeleted);
            if (invitationIsAlreadySent)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} the shop with id {shopId},
                        but the invitation is already sent!
                        More details : {@invitationDto}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    sendInvitation.InvitingUserId,
                    sendInvitation.InvitedUserId,
                    sendInvitation.ShopId,
                    sendInvitation
                );

                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "Your invitation has been already sent before!",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var userIsAlreadyInGroup = await _db.ShopShares.Include(x => x.Shop)
                                                           .ThenInclude(x => x.ShopInvitations)
                                                           .AnyAsync(x => x.UserId == sendInvitation.InvitedUserId.ToString() &&
                                                                                       x.ShopId == sendInvitation.ShopId &&
                                                                                       !x.User.IsDeleted);

            if (userIsAlreadyInGroup)
            {
                _logger.Log
                (
                    LogLevel.Error,
                    """
                        [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} the group with id {ShopId},
                        but the user is already in the group!
                        More details : {@invitationDto}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    sendInvitation.InvitingUserId,
                    sendInvitation.InvitedUserId,
                    sendInvitation.ShopId,
                    sendInvitation
                );

                return Util_GenericResponse<bool>.Response
                (
                   false,
                   false,
                   "User is already memeber of the group",
                   null,
                   System.Net.HttpStatusCode.BadRequest
                );
            }

            var invitation = new ShopInvitation
            {
                InvitingUserId = sendInvitation.InvitingUserId.ToString(),
                InvitedUserId = sendInvitation.InvitedUserId.ToString(),
                ShopId = sendInvitation.ShopId,
                InvitationStatus = InvitationStatus.Pending,
                CreatedAt = DateTime.Now,
                EncryptedKey = sendInvitation.EncryptedKey,
                EncryptedKeyNonce = sendInvitation.EncryptedKeyNonce,
            };

            await _db.ShopInvitations.AddAsync(invitation);
            await _db.SaveChangesAsync();

            _logger.Log
            (
                LogLevel.Information,
                """
                    [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} the group with id {shopId}.
                    More details : {@invitationDto}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                sendInvitation.InvitingUserId,
                sendInvitation.InvitedUserId,
                sendInvitation.ShopId,
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
            return await Util_LogsHelper<bool, ShopInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong,
                    user with [ID] {sendInvitation.InvitingUserId} tried to send an invite to user with
                    [ID] {sendInvitation.InvitedUserId} to the shop with [ID] {sendInvitation.ShopId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>> AcceptInvitation(DTO_InvitationRequestActions accepInvitation)
    {
        try
        {
            var checkPassed = await GenerealChecks(accepInvitation.ShopId, accepInvitation.InvitingUserId, accepInvitation.InvitedUserId);

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

            var invitation = await _db.ShopInvitations.FirstOrDefaultAsync(x => x.Id == accepInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == accepInvitation.InvitingUserId.ToString() &&
                invitation.ShopId == accepInvitation.ShopId &&
                invitation.InvitedUserId == accepInvitation.InvitedUserId.ToString() &&
                invitation.InvitationStatus == InvitationStatus.Pending
            )
            {
                invitation.InvitationStatus = InvitationStatus.Accepted;
                await _db.SaveChangesAsync();

                var groupMember = new ShopShare
                {
                    CreatedAt = DateTime.Now,
                    ShopId = invitation.ShopId,
                    UserId = invitation.InvitedUserId,
                    EncryptedKeyNonce = invitation.EncryptedKeyNonce,
                    EncryptedKey = invitation.EncryptedKey,
                };

                await _db.ShopShares.AddAsync(groupMember);
                _db.Remove(invitation);
                await _db.SaveChangesAsync();

                _logger.Log
                (
                    LogLevel.Information,
                    """
                        Request with IP {IP}.
                        User with id {invitedUserId} accepted the invitation with id {invitationId} made by 
                        the user with id {invitingUserId} for the shop with id {shopId}.
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    accepInvitation.InvitedUserId,
                    accepInvitation.InvitationId,
                    accepInvitation.InvitingUserId,
                    accepInvitation.ShopId
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
                     the user with id {invitingUserId} for the group with id {shopId} but the invitation with that 
                     id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                accepInvitation.InvitedUserId,
                accepInvitation.InvitationId,
                accepInvitation.InvitingUserId,
                accepInvitation.ShopId,
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
            return await Util_LogsHelper<bool, ShopInvitations>.ReturnInternalServerError
            (
               ex,
               _logger,
               $"""
                    Something went wrong in,
                    user with [ID] {accepInvitation.InvitedUserId} tried to accept the invitation by user with id {accepInvitation.InvitingUserId} for the 
                    shop with id {accepInvitation.ShopId} with the invitation id {accepInvitation.InvitationId}.
                """,
               false,
               _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>> RejectInvitation(DTO_InvitationRequestActions rejectInvitation)
    {
        try
        {
            var checkPassed = await GenerealChecks(rejectInvitation.ShopId, rejectInvitation.InvitingUserId, rejectInvitation.InvitedUserId);

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

            var invitation = await _db.ShopInvitations.FirstOrDefaultAsync(x => x.Id == rejectInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == rejectInvitation.InvitingUserId.ToString() &&
                invitation.ShopId == rejectInvitation.ShopId &&
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
                         the user with id {invitingUserId} for the shop with id {shopId}. Dto {@DTO}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    rejectInvitation.InvitedUserId,
                    rejectInvitation.InvitationId,
                    rejectInvitation.InvitingUserId,
                    rejectInvitation.ShopId,
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
                     the user with id {invitingUserId} for the shop with id {shopId} but the invitation with that 
                     id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                rejectInvitation.InvitedUserId,
                rejectInvitation.InvitationId,
                rejectInvitation.InvitingUserId,
                rejectInvitation.ShopId,
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
            return await Util_LogsHelper<bool, ShopInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong in [GroupManagment Module]-[GroupManagment_GroupInvitationsRepository class]-[RejectInvitation Method],
                    user with [ID] {rejectInvitation.InvitedUserId} tried to reject the invitation by user with id {rejectInvitation.InvitingUserId} for the 
                    shop with id {rejectInvitation.ShopId} with the invitation id {rejectInvitation.InvitationId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    public async Task<Util_GenericResponse<bool>> DeleteSentInvitation(DTO_InvitationRequestActions deleteInvitation)
    {
        try
        {
            var checkPassed = await GenerealChecks(deleteInvitation.ShopId, deleteInvitation.InvitingUserId, deleteInvitation.InvitedUserId);

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

            var invitation = await _db.ShopInvitations.FirstOrDefaultAsync(x => x.Id == deleteInvitation.InvitationId);

            if
            (
                invitation != null &&
                invitation.InvitingUserId == deleteInvitation.InvitingUserId.ToString() &&
                invitation.ShopId == deleteInvitation.ShopId &&
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
                        the user with id {invitedUserId} for the shop with id {shopId}. Dto {@DTO}
                     """,
                    await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                    deleteInvitation.InvitingUserId,
                    deleteInvitation.InvitationId,
                    deleteInvitation.InvitedUserId,
                    deleteInvitation.ShopId,
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
                    the user with id {invitedUserId} for the shop with id {shopId} but the invitation with that 
                    id doesn't exists. Dto {@DTO}
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                deleteInvitation.InvitingUserId,
                deleteInvitation.InvitationId,
                deleteInvitation.InvitedUserId,
                deleteInvitation.ShopId,
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
            return await Util_LogsHelper<bool, ShopInvitations>.ReturnInternalServerError
            (
                ex,
                _logger,
                $"""
                    Something went wrong,
                    user with [ID] {deleteInvitation.InvitingUserId} tried to delete the invitation to the user with id {deleteInvitation.InvitedUserId} for the 
                    shop with id {deleteInvitation.ShopId} with the invitation id {deleteInvitation.InvitationId}.
                 """,
                false,
                _httpContextAccessor
            );
        }
    }

    private async Task<bool> GenerealChecks(Guid shopId, Guid invitingUserId, Guid invitedUserId)
    {
        var shop = await _db.Shops.FirstOrDefaultAsync(x => x.ShopId == shopId);

        if (shop is null)
        {
            _logger.Log
            (
                LogLevel.Error,
                """
                    [RESULT] : [IP] {IP} user with [ID] {ID} invited user with id {InvitedUserId} to a non existing or deleted shop.
                 """,
                await Util_GetIpAddres.GetLocation(_httpContextAccessor),
                invitingUserId,
                invitedUserId
            );

            return false;
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

        return true;
    }
}