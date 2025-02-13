using AutoMapper;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Dto.Authentication;

namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateServiceTypeDto, ServiceType>()
            .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceTypeName));

        CreateMap<UpdateServiceTypeDto, ServiceType>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
       
        CreateMap<CreateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => src.ServiceTypeId))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))); //Meaning of 7.0 is GMT+7

        CreateMap<UpdateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))) //Meaning of 7.0 is GMT+7
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Ignore null value

        /*
        CreateMap<CreateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))) // UTC+7
            // TODO: Change this to the actual user ID
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticBlogStatus.Created));

        CreateMap<BlogCategory, GetBlogCategoryDto>().ReverseMap();

        CreateMap<UpdateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.BlogCategoryId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))) // UTC+7
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticBlogStatus.Modified));
            */
        
        //SignUpCustomerDto to ApplicationUser
        CreateMap<SignUpCustomerDto, ApplicationUser>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>src.Gender))
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
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
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
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

        //UpdateUserProfileDto to ApplicationUser
        CreateMap<UpdateUserProfileDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ReverseMap();
    }
}