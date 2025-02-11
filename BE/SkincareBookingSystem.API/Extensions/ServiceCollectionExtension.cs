using Microsoft.AspNetCore.Identity;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Mapping;
using SkincareBookingSystem.Services.Services;
using StackExchange.Redis;

namespace SkincareBookingSystem.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services,ConfigurationManager builderConfiguration)
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

            return services;
        }
    }
}
