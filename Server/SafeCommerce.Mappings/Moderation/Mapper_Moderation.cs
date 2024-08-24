using AutoMapper;
using SafeCommerce.DataTransormObject.ModerationHistory;

namespace SafeCommerce.Mappings.Moderation;

public class Mapper_Moderation : Profile
{
    public Mapper_Moderation()
    {
        CreateMap<SafeCommerce.DataAccessLayer.Models.ModerationHistory, DTO_ModerationHistory>()
            .ForMember(dest => dest.ShopId, opt => opt.MapFrom(src => src.ShopId))
            .ForMember(dest => dest.ModeratorId, opt => opt.MapFrom(src => src.ModeratorId))
            .ForMember(dest => dest.Approved, opt => opt.MapFrom(src => src.Approved))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.ModeratorName, opt => opt.MapFrom(src => src.Moderator!.FullName))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.Item.Name))
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop!.Name))
            .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.CreatedAt));
    }
}