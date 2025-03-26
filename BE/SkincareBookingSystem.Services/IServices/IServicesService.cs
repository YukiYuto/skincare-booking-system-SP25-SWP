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

        Task<ResponseDto> GetAllServices
        (
            ClaimsPrincipal User,
            int pageNumber = 1,
            int pageSize = 10,
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null
        );

        Task<ResponseDto> CreateService(ClaimsPrincipal User, CreateServiceDto createServiceDto);
        Task<ResponseDto> CreateBulkServices(ClaimsPrincipal User, List<CreateServiceDto> createServiceDtos);
        Task<ResponseDto> UpdateService(ClaimsPrincipal User, UpdateServiceDto updateServiceDto);
        Task<ResponseDto> DeleteService(Guid id);
    }
}