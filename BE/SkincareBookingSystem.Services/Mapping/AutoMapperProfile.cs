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
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Models.Dto.Customer;
using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Blog;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;
using SkincareBookingSystem.Models.Dto.Payment;



namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        //Blog 
        CreateMap<CreateBlogDto, Blog>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.BlogCategoryId, opt => opt.MapFrom(src => src.BlogCategoryId))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));
        CreateMap<UpdateBlogDto, Blog>() 
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

        // BookingService.BundleOrder - Order response to avoid cyclic references when returned
        CreateMap<Order, Models.Dto.Booking.Order.OrderDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

        CreateMap<OrderDetail, Models.Dto.Booking.Order.OrderDetailDto>();


        //BookingSchedule
        CreateMap<CreateTherapistScheduleDto, TherapistSchedule>();
        CreateMap<UpdateTherapistScheduleDto, TherapistSchedule>();

        CreateMap<UpdateOrderDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        //OrderDetail
        CreateMap<UpdateOrderDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<BundleOrderDto, Order>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Order.CustomerId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Order.TotalPrice))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Order.Created));


        CreateMap<CreateOrderDetailDto, OrderDetail>()
            .ForMember(dest => dest.OrderDetailId, opt => opt.MapFrom(src => Guid.NewGuid()))
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

        //ServiceType
        CreateMap<CreateServiceTypeDto, ServiceType>()
            .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceTypeName));

        CreateMap<UpdateServiceTypeDto, ServiceType>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        //Service
        CreateMap<Models.Domain.Services, GetAllServicesDto>().ReverseMap();
        
        CreateMap<CreateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => src.ServiceTypeId))
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Service.Active));
        
        CreateMap<UpdateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.UpdatedTime,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0)))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

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

        // ApplicationUser to GetCustomerDto
        CreateMap<Customer, GetCustomerDto>()
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.ApplicationUser.Age))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ApplicationUser.Gender))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
           .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.ApplicationUser.Address))
           .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl))
           .ForMember(dest => dest.SkinProfileId, opt => opt.MapFrom(src => src.SkinProfileId));
        // SkinTherapist to GetSkinTherapistDto
        CreateMap<SkinTherapist, GetSkinTherapistDto>()
           .ForMember(dest => dest.SkinTherapistId, opt => opt.MapFrom(src => src.SkinTherapistId))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
           .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.ApplicationUser.Age))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.ApplicationUser.Gender))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
           .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl))
           .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience));


        // Slots
        CreateMap<CreateSlotDto, Slot>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        CreateMap<UpdateSlotDto, Slot>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null));

        // Appointments
        CreateMap<CreateAppointmentDto, Appointments>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Appointment.Created))
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam)); // UTC+7

        CreateMap<UpdateAppointmentDto, Appointments>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember is not null));

        CreateMap<Guid, TherapistServiceType>()
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => src));

        CreateMap<SkinTherapist, Models.Dto.TherapistServiceTypes.TherapistDto>()
            .ForMember(dest => dest.SkinTherapistId, opt => opt.MapFrom(src => src.SkinTherapistId))
            .ForMember(dest => dest.ServiceTypes, opt => opt.MapFrom(
                src => src.TherapistServiceTypes.Select(tst => tst.ServiceType).ToList()));

        CreateMap<ServiceType, Models.Dto.TherapistServiceTypes.ServiceTypeDto>();
        
        //Payment
        CreateMap<GetAllPaymentDto, Payment>().ReverseMap();
        CreateMap<GetPaymentByIdDto, Payment>().ReverseMap();

    }
}
