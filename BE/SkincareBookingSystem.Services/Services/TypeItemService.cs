using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TypeItem;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class TypeItemService : ITypeItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _mapper;
    
    public TypeItemService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ResponseDto> CreateTypeItem(ClaimsPrincipal user, CreateTypeItemDto createTypeItemDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null)
        {
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };
        }
        
        if (createTypeItemDto.ServiceTypeIdList == null || !createTypeItemDto.ServiceTypeIdList.Any())
        {
            return new ResponseDto
            {
                Message = "ServiceType cannot be empty",
                IsSuccess = false,
                StatusCode = 400
            };
        }
        
        var typeItems = createTypeItemDto.ServiceTypeIdList.Select(sp => new TypeItem
        {
            ServiceId = createTypeItemDto.ServiceId,
            ServiceTypeId = sp
        }).ToList();
        
        await _unitOfWork.TypeItem.AddRangeAsync(typeItems);
        await _unitOfWork.SaveAsync();
        
        return new ResponseDto()
        {
            Message = "Create Type Item Successful",
            IsSuccess = true,
            StatusCode = 201,
            Result = typeItems
        };
    }

    public Task<ResponseDto> RemoveTypeItem(ClaimsPrincipal user, CreateTypeItemDto removeTypeItemDto)
    {
        return null;
    }
}