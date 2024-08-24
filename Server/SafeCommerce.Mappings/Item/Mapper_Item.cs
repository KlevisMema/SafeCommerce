using AutoMapper;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.Mappings.Item;
public class Mapper_Item : Profile
{
    public Mapper_Item()
    {
        CreateMap<SafeCommerce.DataAccessLayer.Models.Item, DTO_ItemForModeration>()
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.Picture, opt => opt.MapFrom(src => src.Picture))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ShopName, opt => opt.MapFrom(src => src.Shop.Name))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.FullName))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        
        CreateMap<SafeCommerce.DataAccessLayer.Models.Item, DTO_Item>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.OwnerPublicKey, opt => opt.MapFrom(src => src.Owner.PublicKey))
            .ForMember(dest => dest.OwnerSignature, opt => opt.MapFrom(src => src.Owner.Signature))
            .ForMember(dest => dest.OwnerSigningPublicKey, opt => opt.MapFrom(src => src.Owner.SigningPublicKey));

        CreateMap<DTO_CreateItem, SafeCommerce.DataAccessLayer.Models.Item>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

        CreateMap<DTO_UpdateItem, SafeCommerce.DataAccessLayer.Models.Item>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());
    }
}