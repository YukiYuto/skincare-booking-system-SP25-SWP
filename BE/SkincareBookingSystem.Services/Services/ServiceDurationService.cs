using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceDuration;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class ServiceDurationService : IServiceDurationService
{
    private readonly IAutoMapperService _autoMapperService;
    private readonly IUnitOfWork _unitOfWork;

    public ServiceDurationService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
    {
        _unitOfWork = unitOfWork;
        _autoMapperService = autoMapperService;
    }


    public async Task<ResponseDto> GetAllServiceDurations
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        var (serviceDuration, totalServiceDuration) =
            await _unitOfWork.ServiceDuration.GetAllServiceDurationAsync
            (
                pageNumber,
                pageSize,
                filterOn,
                filterQuery,
                sortBy,
                true
            );

        if (!serviceDuration.Any())
            return new ResponseDto
            {
                Result = serviceDuration,
                Message = "No Service Durations found",
                IsSuccess = false,
                StatusCode = 404
            };

        return new ResponseDto
        {
            Result = serviceDuration,
            Message = "Service Durations retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> CreateServiceDuration(ClaimsPrincipal User,
        CreateServiceDurationDto createServiceDurationDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        var serviceDuration =
            _autoMapperService.Map<CreateServiceDurationDto, ServiceDuration>(createServiceDurationDto);

        await _unitOfWork.ServiceDuration.AddAsync(serviceDuration);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            Result = serviceDuration,
            Message = "Service Duration created successfully",
            IsSuccess = true,
            StatusCode = 201
        };
    }
}