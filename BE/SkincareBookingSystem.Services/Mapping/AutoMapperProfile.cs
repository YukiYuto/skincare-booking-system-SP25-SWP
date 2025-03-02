using AutoMapper;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Models.Dto.Customer;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;

namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<UpdateOrderDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<BundleOrderDto, Order>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Order.CustomerId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Order.TotalPrice))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Order.Created));

        CreateMap<CreateOrderDetailDto, OrderDetail>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dest => dest.ServiceComboId, opt => opt.MapFrom(src => src.ServiceComboId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<SkinTherapist, PreviewTherapistDto>()
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.SkinTherapistId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.ApplicationUser.Age))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl))
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience));

        CreateMap<UpdateOrderDetailDto, OrderDetail>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

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
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))); //Meaning of 7.0 is GMT+7

        CreateMap<UpdateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.UpdatedTime,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0))) //Meaning of 7.0 is GMT+7
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
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => string.Empty))
            .ForMember(dest => dest.LockoutEnabled, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

        //SignUpCustomerDto to Customer
        CreateMap<SignUpCustomerDto, Customer>()
            .ForMember(dest => dest.SkinProfileId, opt => opt.Ignore());

        //SignUpSkinTherapistDto to SkinTherapist
        CreateMap<SignUpSkinTherapistDto, SkinTherapist>()
            .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience));

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
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ReverseMap();


        // Slots
        CreateMap<CreateSlotDto, Slot>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        CreateMap<UpdateSlotDto, Slot>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null));

        // Appointments
        CreateMap<CreateAppointmentDto, Appointments>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Appointment.Created))
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam)); // UTC+7

        CreateMap<UpdateAppointmentDto, Appointments>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null));

        //! TODO: Uncomment this when the TherapistServiceType is updated to inherit BaseEntity
        CreateMap<Guid, TherapistServiceType>()
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // We'll set this manually
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(_ => StaticOperationStatus.Timezone.Vietnam))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => StaticOperationStatus.BaseEntity.Active));
    }
}