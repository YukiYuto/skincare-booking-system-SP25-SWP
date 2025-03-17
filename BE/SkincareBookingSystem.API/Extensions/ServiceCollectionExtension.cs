using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Mapping;
using SkincareBookingSystem.Services.Services;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Invoker;
using StackExchange.Redis;

namespace SkincareBookingSystem.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        // Đọc chuỗi kết nối Redis từ file cấu hình
        var redisConnectionString = builderConfiguration.GetValue<string>("Redis:ConnectionString");
        // Đăng ký IConnectionMultiplexer
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        // Register AutoMapper
        services.AddAutoMapper(typeof(AutoMapperProfile));

        // Scoped services
        services.AddScoped<IAutoMapperService, AutoMapperService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();

        // more services to be added here
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRedisService, RedisService>();
        services.AddScoped<IBlogCategoryService, BlogCategoryService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<CloudinaryServiceControl>();

        // more services to be added here
        services.AddScoped<IServiceTypeService, ServiceTypeService>();
        services.AddScoped<IServicesService, ServicesService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IServiceComboService, ServiceComboService>();
        services.AddScoped<IComboItemService, ComboItemService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<ITypeItemService, TypeItemService>();
        services.AddScoped<IServiceDurationService, ServiceDurationService>();
        services.AddScoped<IDurationItemService, DurationItemService>();

        services.AddScoped<ISkinTherapistService, SkinTherapistService>();
        services.AddScoped<ITherapistServiceTypeService, TherapistServiceTypeService>();

        services.AddScoped<IOrderDetailService, OrderDetailService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<ITherapistScheduleService, TherapistScheduleService>();

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<ITestAnswerService, TestAnswerService>();
        services.AddScoped<ITestQuestionService, TestQuestionService>();
        services.AddScoped<IFeedbackService, FeedbackService>();
        services.AddScoped<IBlogService, BlogService>();

        // add services related to Repository
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ITherapistScheduleRepository, TherapistScheduleRepository>();

        return services;
    }
}