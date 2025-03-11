using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.ServiceCombo;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class ServiceComboService : IServiceComboService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _mapper;
    
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

    public Task<ResponseDto> GetAllServiceCombos(ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetServiceComboById(ClaimsPrincipal user, Guid serviceComboId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> DeleteServiceCombo(ClaimsPrincipal user, Guid serviceComboId)
    {
        throw new NotImplementedException();
    }
}