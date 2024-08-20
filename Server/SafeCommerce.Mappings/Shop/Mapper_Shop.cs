using AutoMapper;
using SafeCommerce.DataTransormObject.Shop;
using SafeShare.DataAccessLayer.Models;

namespace SafeCommerce.Mappings.Shop;
public class Mapper_Shop : Profile
{
    public Mapper_Shop()
    {
        CreateMap<SafeShare.DataAccessLayer.Models.Shop, DTO_Shop>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.ShopShares, opt => opt.MapFrom(src => src.ShopShares))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.OwnerPublicKey, opt => opt.MapFrom(src => src.Owner.PublicKey))
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src =>
                src.ShopShares != null ? src.ShopShares.Count : 0))
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src =>
                src.Items != null ? src.Items.Count : 0));

        CreateMap<SafeShare.DataAccessLayer.Models.Shop, DTO_ShopForModeration>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            

        CreateMap<ShopShare, DTO_ShopShare>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

        CreateMap<DTO_CreateShop, SafeShare.DataAccessLayer.Models.Shop>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.EncryptedKey, opt => opt.MapFrom(src => src.EncryptedSymmetricKey));

        CreateMap<DTO_UpdateShop, SafeShare.DataAccessLayer.Models.Shop>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());
    }
}