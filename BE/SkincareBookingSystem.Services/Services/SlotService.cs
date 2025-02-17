using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class SlotService : ISlotService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAutoMapperService _autoMapperService;

    public SlotService(IUnitOfWork unitOfWork, IAutoMapperService autoMapperService)
    {
        _unitOfWork = unitOfWork;
        _autoMapperService = autoMapperService;
    }

    public Task<ResponseDto> GetAllSlots()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetSlot(Guid slotId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<ResponseDto> CreateSlot(ClaimsPrincipal user, CreateSlotDto slotDto)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = "User not found",
                StatusCode = 404,
                Result = null
            };
        }
        //map to dto
        Slot slot = _autoMapperService.Map<CreateSlotDto, Slot>(slotDto);
        
        //add slot to baseEntity
        slot.CreatedBy = user.FindFirstValue("FullName");
        slot.CreatedTime = DateTime.UtcNow;
        slot.Status = "1";
        
        //Requirement to create slot
        if (slotDto.EndTime <= slotDto.StartTime)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "EndTime must be later than StartTime.",
                StatusCode = 400,
                Result = null
            };
        }

        TimeSpan duration = slotDto.EndTime - slotDto.StartTime;
        if (duration < TimeSpan.FromHours(1))
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "The duration between StartTime and EndTime must be at least one hour.",
                StatusCode = 400,
                Result = null
            };
        }
        
        //add slot to db
        await _unitOfWork.Slot.AddAsync(slot);
        
        //save db and check if success
        var result = await _unitOfWork.SaveAsync();
        if (result < 1)
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = "Failed to create slot",
                StatusCode = 404,
                Result = null
            };
        }

        return new ResponseDto()
        {
            IsSuccess = true,
            Message = "Slot created successfully",
            StatusCode = 201,
            Result = slot
        };
    }
    

    public Task<ResponseDto> UpdateSlot(ClaimsPrincipal user, UpdateSlotDto slotDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> DeleteSlot(ClaimsPrincipal user, Guid slotId)
    {
        throw new NotImplementedException();
    }
}