using AutoMapper;
using SafeCommerce.DataTransormObject.Item;

namespace SafeCommerce.Mappings.Item;
public class Mapper_Item : Profile
{
    public Mapper_Item()
    {
        CreateMap<SafeShare.DataAccessLayer.Models.Item, DTO_Item>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.UserName))
            .ForMember(dest => dest.OwnerPublicKey, opt => opt.MapFrom(src => src.Owner.PublicKey))
            .ForMember(dest => dest.OwnerSignature, opt => opt.MapFrom(src => src.Owner.Signature))
            .ForMember(dest => dest.OwnerSigningPublicKey, opt => opt.MapFrom(src => src.Owner.SigningPublicKey));

        CreateMap<DTO_CreateItem, SafeShare.DataAccessLayer.Models.Item>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

        CreateMap<DTO_UpdateItem, SafeShare.DataAccessLayer.Models.Item>()
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());
    }
}