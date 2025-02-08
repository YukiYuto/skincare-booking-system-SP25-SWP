using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Authentication;

namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //SignUpCustomerDto to ApplicationUser
        CreateMap<SignUpCustomerDto, ApplicationUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));
        
        //SignUpCustomerDto to Customer
        CreateMap<SignUpCustomerDto, Customer>()
            .ForMember(dest => dest.SkinProfileId, opt => opt.Ignore());
        
        //SignUpSkinTherapistDto to SkinTherapist
        CreateMap<SignUpSkinTherapistDto, SkinTherapist>()
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.TherapistScheduleId, opt => opt.Ignore());
        
        //SignUpSkinTherapistDto to ApplicationUser
        CreateMap<SignUpSkinTherapistDto, ApplicationUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));
        
        //SignUpStaffDto to ApplicationUser
        CreateMap<SignUpStaffDto, ApplicationUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));
        
    }
}