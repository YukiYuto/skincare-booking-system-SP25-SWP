using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Services.Services;

public class ComboItemService : IComboItemService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _mapper;
    
    public ComboItemService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<ResponseDto> CreateComboItem(ClaimsPrincipal user, CreateComboItemDto createComboItemDto)
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
        
        if (createComboItemDto.ServicePriorityDtos == null || !createComboItemDto.ServicePriorityDtos.Any())
        {
            return new ResponseDto
            {
                Message = "ServicePriorityDtos cannot be empty",
                IsSuccess = false,
                StatusCode = 400
            };
        }

        var comboItems = createComboItemDto.ServicePriorityDtos.Select(sp => new ComboItem
        {
            ServiceComboId = createComboItemDto.ServiceComboId,
            ServiceId = sp.ServiceId,
            Priority = sp.Priority
        }).ToList();
        
        //var comboItem = _mapper.Map<CreateComboItemDto, ComboItem>(createComboItemDto);
        await _unitOfWork.ComboItem.AddRangeAsync(comboItems);
        await _unitOfWork.SaveAsync();

        return new ResponseDto()
        {
            Message = "Create Combo Item Successful",
            IsSuccess = true,
            StatusCode = 201,
            Result = comboItems
        };
    }
}