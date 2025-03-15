using SkincareBookingSystem.DataAccess.Repositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceTypeDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IServiceTypeService
    {
        Task<ResponseDto> GetServiceTypeById(Guid id);
        Task<ResponseDto> GetAllServiceTypes();
        Task<ResponseDto> CreateServiceType(ClaimsPrincipal User, CreateServiceTypeDto createServiceTypeDto);
        Task<ResponseDto> CreateBulkServiceTypes(ClaimsPrincipal User, List<CreateServiceTypeDto> createServiceTypeDtos);
        Task<ResponseDto> UpdateServiceType(ClaimsPrincipal User, UpdateServiceTypeDto updateServiceTypeDto);
        Task<ResponseDto> DeleteServiceType(Guid id);
    }
}
