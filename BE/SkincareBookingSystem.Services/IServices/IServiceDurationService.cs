using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceDuration;

namespace SkincareBookingSystem.Services.IServices;

public interface IServiceDurationService
{
    Task<ResponseDto> GetAllServiceDurations
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );

    Task<ResponseDto> CreateServiceDuration(ClaimsPrincipal User, CreateServiceDurationDto createServiceDurationDto);
}