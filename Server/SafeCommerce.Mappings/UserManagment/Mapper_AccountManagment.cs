/* 
 * Provides AutoMapper mappings for the user management module.
*/

using AutoMapper;
using SafeCommerce.DataAccess.Models;
using SafeCommerce.DataTransormObject.UserManagment;

namespace SafeCommerce.Mappings.UserManagment;

/// <summary>
/// Defines the mapping profiles for the Account Management between data models and DTOs.
/// </summary>
public class Mapper_AccountManagment : Profile
{
    /// <summary>
    /// Initializes the mapping definitions.
    /// </summary>
    public Mapper_AccountManagment()
    {
        // Map from ApplicationUser to DTO_UserInfo
        CreateMap<ApplicationUser, DTO_UserInfo>();

        // Map from DTO_UserInfo to ApplicationUser with custom property mappings
        CreateMap<DTO_UserInfo, ApplicationUser>()
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.UtcNow.Year - src.Birthday.Year));

        // Map from ApplicationUser to DTO_UserUpdatedInfo with custom property mappings
        CreateMap<ApplicationUser, DTO_UserUpdatedInfo>()
           .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ImageData));

        CreateMap<ApplicationUser, DTO_UserSearched>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
    }
}