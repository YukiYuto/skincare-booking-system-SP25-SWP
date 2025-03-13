using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.DurationItem;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class DurationItemService : IDurationItemService
{
    private readonly IAutoMapperService _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DurationItemService(IUnitOfWork unitOfWork, IAutoMapperService mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseDto> AddDurationItemAsync(ClaimsPrincipal User, CreateDurationItemDto durationItemDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        var durationDto = _mapper.Map<CreateDurationItemDto, DurationItem>(durationItemDto);
        if (durationDto is null)
            return new ResponseDto
            {
                Message = "Invalid duration item",
                IsSuccess = false,
                StatusCode = 400
            };

        await _unitOfWork.DurationItem.AddAsync(durationDto);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Duration item added successfully",
            StatusCode = 201,
            Result = durationDto
        };
    }

    public async Task<ResponseDto> GetAllDurationItem(ClaimsPrincipal User, int pageNumber, int pageSize)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return new ResponseDto
            {
                Message = "User not found",
                IsSuccess = false,
                StatusCode = 404
            };

        var (durationItems, totalDurationItems) = await _unitOfWork.DurationItem.GetAllDurationItemsAsync
        (
            pageNumber,
            pageSize
        );

        if (!durationItems.Any())
            return new ResponseDto
            {
                Message = "No duration items found",
                IsSuccess = false,
                StatusCode = 404
            };

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "All duration items",
            StatusCode = 200,
            Result = durationItems
        };
    }

    public async Task<ResponseDto> DeleteDuration(CreateDurationItemDto durationItemDto)
    {
        var duration = await _unitOfWork.DurationItem.GetAsync(d =>
            d.ServiceId == durationItemDto.ServiceId &&
            d.ServiceDurationId == durationItemDto.ServiceDurationId);
        if (duration == null)
            return new ResponseDto
            {
                Message = "Duration not found",
                IsSuccess = false,
                StatusCode = 404
            };

        _unitOfWork.DurationItem.Remove(duration);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Duration deleted successfully",
            StatusCode = 200
        };
    }
}