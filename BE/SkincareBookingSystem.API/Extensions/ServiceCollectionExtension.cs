using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Services;

namespace SkincareBookingSystem.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // more services to be added here
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBlogCategoryService, BlogCategoryService>();
            services.AddScoped<IServicesService, ServicesService>();

            // 3rd party services
            services.AddScoped<IAutoMapperService, AutoMapperService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            return services;
        }
    }
}
