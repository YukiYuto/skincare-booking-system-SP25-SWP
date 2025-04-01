using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceCombo;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services;

public class ServiceComboService : IServiceComboService
{
    private readonly IAutoMapperService _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public ServiceComboService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> CreateServiceCombo(ClaimsPrincipal user, CreateServiceComboDto createServiceComboDto)
    {
        try
        {
            var service = _mapper.Map<CreateServiceComboDto, ServiceCombo>(createServiceComboDto);
            service.CreatedBy = user.FindFirstValue("FullName");
            service.NumberOfService = 0;

            await _unitOfWork.ServiceCombo.AddAsync(service);
            await _unitOfWork.SaveAsync();

            return new ResponseDto
            {
                Result = service,
                Message = "ServiceCombo(s) created successfully",
                IsSuccess = true,
                StatusCode = 201
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                Message = ex.Message,
                IsSuccess = false,
                StatusCode = 500
            };
        }
    }

    public Task<ResponseDto> UpdateServiceCombo(ClaimsPrincipal user, UpdateServiceComboDto updateServiceComboDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDto> GetAllServiceCombos
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    )
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        var isManager = userRole == StaticUserRoles.Manager;

        var (serviceCombo, total) =
            await _unitOfWork.ServiceCombo.GetAll(pageNumber, pageSize, filterOn, filterQuery, sortBy, isManager);

        var filteredServiceCombos = serviceCombo.Where(sc => sc.Status == StaticOperationStatus.Service.Active);
        var filteredTotal = filteredServiceCombos.Count();

        if (!filteredServiceCombos.Any())
            return new ResponseDto
            {
                Result = serviceCombo,
                Message = "No ServiceCombo found",
                IsSuccess = false,
                StatusCode = 200
            };

        return new ResponseDto
        {
            Result = new
            {
                TotalServiceCombos = filteredTotal,
                TotalPages = (int)Math.Ceiling((double)filteredTotal / pageSize),
                PageSize = pageSize,
                CurrentPage = pageNumber,
                ServiceCombos = filteredServiceCombos
            },
            Message = "ServiceCombo(s) retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }


    public async Task<ResponseDto> GetServiceComboById(ClaimsPrincipal user, Guid serviceComboId)
    {
        var serviceCombo = await _unitOfWork.ServiceCombo.GetAsync(s => s.ServiceComboId == serviceComboId,
            includeProperties: $"{nameof(ServiceCombo.ComboItems)}.{nameof(ComboItem.Services)}");
        if (serviceCombo == null)
            return new ResponseDto
            {
                Result = serviceCombo,
                Message = "No ServiceCombo found",
                IsSuccess = false,
                StatusCode = 200
            };
        var servicesFromCombo = serviceCombo.ComboItems.Select(c => c.Services).ToList();
        var serviceDtos = _mapper.MapCollection<Models.Domain.Services, ServiceDto>(servicesFromCombo);
        var serviceComboDto = _mapper.Map<ServiceCombo, ServiceComboDto>(serviceCombo);
        serviceComboDto.Services = serviceDtos.ToList();

        return new ResponseDto
        {
            Result = serviceComboDto,
            Message = "ServiceCombo retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public Task<ResponseDto> DeleteServiceCombo(ClaimsPrincipal user, Guid serviceComboId)
    {
        throw new NotImplementedException();
    }
}