using AutoMapper;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Appointment;
using SkincareBookingSystem.Models.Dto.Appointment.Details;
using SkincareBookingSystem.Models.Dto.Authentication;
using SkincareBookingSystem.Models.Dto.Blog;
using SkincareBookingSystem.Models.Dto.BlogCategories;
using SkincareBookingSystem.Models.Dto.Booking.Appointment;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment;
using SkincareBookingSystem.Models.Dto.Booking.Order;
using SkincareBookingSystem.Models.Dto.Booking.SkinTherapist;
using SkincareBookingSystem.Models.Dto.BookingSchedule;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Models.Dto.Customer;
using SkincareBookingSystem.Models.Dto.DurationItem;
using SkincareBookingSystem.Models.Dto.Feedbacks;
using SkincareBookingSystem.Models.Dto.GetCustomerInfo;
using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Orders;
using SkincareBookingSystem.Models.Dto.Payment;
using SkincareBookingSystem.Models.Dto.ServiceCombo;
using SkincareBookingSystem.Models.Dto.ServiceDuration;
using SkincareBookingSystem.Models.Dto.Services;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using SkincareBookingSystem.Models.Dto.SkinTherapist;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Models.Dto.TestAnswer;
using SkincareBookingSystem.Models.Dto.TestQuestion;
using SkincareBookingSystem.Models.Dto.TherapistSchedules;
using SkincareBookingSystem.Models.Dto.TherapistServiceTypes;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Customer, CustomerInfoDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.ApplicationUser.Email));

        CreateMap<Models.Domain.Services, ServiceInfoDto>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
            .ForMember(dest => dest.ServicePrice, opt => opt.MapFrom(src => src.Price));

        CreateMap<SkinTherapist, TherapistInfoDto>()
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.SkinTherapistId))
            .ForMember(dest => dest.TherapistName, opt => opt.MapFrom(src => src.ApplicationUser.FullName))
            .ForMember(dest => dest.TherapistAge, opt => opt.MapFrom(src => src.ApplicationUser.Age))
            .ForMember(dest => dest.TherapistAvatarUrl, opt => opt.MapFrom(src => src.ApplicationUser.ImageUrl));

        CreateMap<Appointments, AppointmentDetailsDto>()
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

        // GetCustomerInfo
        CreateMap<Customer, GetCustomerInfoDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ApplicationUser.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.ApplicationUser.FullName));

        //ServiceDuration 
        CreateMap<CreateServiceDurationDto, ServiceDuration>();

        //Duration
        CreateMap<CreateDurationItemDto, DurationItem>();
        CreateMap<GetDurationItemDto, DurationItem>().ReverseMap();

        // Feedbacks
        CreateMap<CreateFeedbackDto, Feedbacks>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId));
        CreateMap<UpdateFeedbackDto, Feedbacks>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // BlogCategory
        CreateMap<CreateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Appointment.Created))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam));
        CreateMap<UpdateBlogCategoryDto, BlogCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Appointment.Created))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam))
            .ForMember(dest => dest.BlogCategoryId, opt => opt.MapFrom(src => src.BlogCategoryId));


        // BookAppointmentDto to Appointments
        CreateMap<BookAppointmentDto, Appointments>()
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Appointment.Created))
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam)); // UTC+7

        // Appointment to AppointmentDto
        CreateMap<Appointments, AppointmentDto>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.AppointmentDate, opt => opt.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.AppointmentTime, opt => opt.MapFrom(src => src.AppointmentTime))
            .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));

        // TherapistSchedule to ScheduleDto
        CreateMap<TherapistSchedule, ScheduleDto>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
            .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));

        //ComboItem
        CreateMap<ComboItem, ServicePriorityDto>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));

        CreateMap<ComboItem, GetComboItemDto>().ReverseMap();

        CreateMap<ServicePriorityDto, ComboItem>()
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority));

        CreateMap<ServiceCombo, GetComboItemDto>()
            .ForMember(dest => dest.ServiceComboId, opt => opt.MapFrom(src => src.ServiceComboId))
            .ForMember(dest => dest.ServicePriorityDtos,
                opt => opt.Ignore()) // Không dùng AutoMapper để ánh xạ danh sách
            .AfterMap((src, dest, context) =>
            {
                dest.ServicePriorityDtos = src.ComboItems
                    .Select(ci => context.Mapper.Map<ServicePriorityDto>(ci)) // Ánh xạ thủ công
                    .ToList();
            });


        //ServiceCombo
        CreateMap<CreateServiceComboDto, ServiceCombo>()
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(src => DateTime.UtcNow.AddHours(7.0)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StaticOperationStatus.Service.Active));

        // TherapistSchedule to ScheduleDto
        CreateMap<TherapistSchedule, ScheduleDto>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
            .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));

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
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => src.CreatedTime))
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

        CreateMap<OrderDetail, OrderDetailDto>();

        //TestAnswer
        CreateMap<CreateTestAnswerDto, TestAnswer>()
            .ForMember(dest => dest.TestQuestionId, opt => opt.MapFrom(src => src.TestQuestionId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
        CreateMap<UpdateTestAnswerDto, TestAnswer>()
            .ForMember(dest => dest.TestAnswerId, opt => opt.MapFrom(src => src.TestAnswerId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.TestQuestionId, opt => opt.MapFrom(src => src.TestQuestionId));

        //TestQuestion
        CreateMap<CreateTestQuestionDto, TestQuestion>()
            .ForMember(dest => dest.SkinTestId, opt => opt.MapFrom(src => src.SkinTestId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
        CreateMap<UpdateTestQuestionDto, TestQuestion>()
            .ForMember(dest => dest.TestQuestionId, opt => opt.MapFrom(src => src.TestQuestionId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.SkinTestId, opt => opt.MapFrom(src => src.SkinTestId));

        //BookingSchedule
        CreateMap<CreateTherapistScheduleDto, TherapistSchedule>()
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));
        CreateMap<UpdateTherapistScheduleDto, TherapistSchedule>()
            .ForMember(dest => dest.TherapistScheduleId, opt => opt.MapFrom(src => src.TherapistScheduleId))
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));
        CreateMap<TherapistSchedule, GetTherapistScheduleDto>()
            .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
            .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
            .ForMember(dest => dest.SlotId, opt => opt.MapFrom(src => src.SlotId));

        //Order
        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

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
            .ForMember(dest => dest.ServiceTypeId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ServiceTypeName, opt => opt.MapFrom(src => src.ServiceTypeName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(src => StaticOperationStatus.Timezone.Vietnam))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "0"));

        CreateMap<UpdateServiceTypeDto, ServiceType>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        //Service
        CreateMap<Models.Domain.Services, GetAllServicesDto>()
            .ForMember(dest => dest.ServiceTypeIds,
                opt => opt.MapFrom(src => src.TypeItems.Select(ti => ti.ServiceTypeId).ToList()))
            .ReverseMap();

        CreateMap<CreateServiceDto, Models.Domain.Services>()
            .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.ServiceName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
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

        CreateMap<SkinTherapist, TherapistDto>()
            .ForMember(dest => dest.SkinTherapistId, opt => opt.MapFrom(src => src.SkinTherapistId))
            .ForMember(dest => dest.ServiceTypes, opt => opt.MapFrom(
                src => src.TherapistServiceTypes.Select(tst => tst.ServiceType).ToList()));

        CreateMap<ServiceType, ServiceTypeDto>();

        //Payment
        CreateMap<GetAllPaymentDto, Payment>().ReverseMap();
        CreateMap<GetPaymentByIdDto, Payment>().ReverseMap();
    }
}