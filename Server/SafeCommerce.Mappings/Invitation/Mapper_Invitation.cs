using AutoMapper;
using SafeCommerce.DataAccessLayer.Models;
using SafeCommerce.DataTransormObject.Invitation;
using SafeCommerce.DataTransormObject.Item;
using SafeShare.DataAccessLayer.Models;


namespace SafeCommerce.Mappings.Invitation;

public class Mapper_Invitation : Profile
{
    public Mapper_Invitation() {
        CreateMap<ShopInvitation, DTO_RecivedInvitations>()
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name))
            .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.ShopId))
            .ForMember(dest => dest.InvitingUserId, opt => opt.MapFrom(src => Guid.Parse(src.InvitingUserId)))
            .ForMember(dest => dest.InvitingUserName, opt => opt.MapFrom(src => src.InvitingUser.FullName))
            .ForMember(dest => dest.InvitationStatus, opt => opt.MapFrom(src => src.InvitationStatus))
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ShopInvitation, DTO_SentInvitations>()
            .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.ShopId))
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name))
            .ForMember(dest => dest.InvitationTimeSend, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.InvitationStatus, opt => opt.MapFrom(src => src.InvitationStatus))
            .ForMember(dest => dest.InvitedUserId, opt => opt.MapFrom(src => src.InvitedUserId))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.InvitedUser.FullName))
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Shop.IsPublic && !src.Shop.MakePublic))
            .ForMember(dest => dest.EncryptedKey, opt => opt.MapFrom(src => src.Shop.EncryptedKey))
            .ForMember(dest => dest.EncryptedKeyNonce, opt => opt.MapFrom(src => src.Shop.EncryptedKeyNonce))
            .ForMember(dest => dest.EncryptedKeyNonce, opt => opt.MapFrom(src => src.Shop.EncryptedKeyNonce))
            .ForMember(dest => dest.DataNonce, opt => opt.MapFrom(src => src.Shop.DataNonce));

        CreateMap<ItemInvitation, DTO_RecivedItemInvitation>()
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.InvitingUserId, opt => opt.MapFrom(src => Guid.Parse(src.InvitingUserId)))
            .ForMember(dest => dest.InvitingUserName, opt => opt.MapFrom(src => src.InvitingUser.FullName))
            .ForMember(dest => dest.InvitationStatus, opt => opt.MapFrom(src => src.InvitationStatus))
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ItemInvitation, DTO_SentItemInvitation>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.InvitationTimeSend, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.InvitationStatus, opt => opt.MapFrom(src => src.InvitationStatus))
            .ForMember(dest => dest.InvitedUserId, opt => opt.MapFrom(src => src.InvitedUserId))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.InvitedUser.FullName))
            .ForMember(dest => dest.InvitationId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsPublic, opt => opt.MapFrom(src => src.Item.IsPublic && !src.Item.MakePublic))
            .ForMember(dest => dest.EncryptedKey, opt => opt.MapFrom(src => src.Item.EncryptedKey))
            .ForMember(dest => dest.EncryptedKeyNonce, opt => opt.MapFrom(src => src.Item.EncryptedKeyNonce))
            .ForMember(dest => dest.EncryptedKeyNonce, opt => opt.MapFrom(src => src.Item.EncryptedKeyNonce))
            .ForMember(dest => dest.DataNonce, opt => opt.MapFrom(src => src.Item.DataNonce));
    }
}