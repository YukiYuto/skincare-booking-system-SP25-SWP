using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IServicesService
    {
        Task<ResponseDto> GetServiceById(Guid id);
        Task<ResponseDto> GetAllServices();
        Task<ResponseDto> CreateService(ClaimsPrincipal User, CreateServiceDto createServiceDto);
        Task<ResponseDto> UpdateService(ClaimsPrincipal User, UpdateServiceDto updateServiceDto);
        Task<ResponseDto> DeleteService(Guid id);

    }
}
