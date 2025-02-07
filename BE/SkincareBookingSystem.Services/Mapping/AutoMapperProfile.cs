using AutoMapper;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.BlogCategory;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
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

    }
}