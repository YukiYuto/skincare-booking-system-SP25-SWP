using System.ComponentModel;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;

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

    public async Task<ResponseDto> GetAllSlots()
    {
        var slotList = await _unitOfWork.Slot.GetAllAsync(slot => slot.Status == StaticOperationStatus.BaseEntity.Active);

        if (!slotList.Any())
        {
            return new ResponseDto()
            {
                IsSuccess = false,
                Message = "No slots found",
                StatusCode = 404,
                Result = null
            };
        }
        return new ResponseDto()
        {
            IsSuccess = true,
            Message = "All slots",
            StatusCode = 200,
            Result = slotList
        };
    }

    public async Task<ResponseDto> GetSlotById(Guid slotId)
    {
        return (await _unitOfWork.Slot.GetAsync(slot => slot.SlotId == slotId) is { } slot) ?
            new ResponseDto
            {
                IsSuccess = true,
                Message = "Slot found",
                StatusCode = 200,
                Result = slot
            }
            : new ResponseDto
            {
                IsSuccess = false,
                Message = "Slot not found",
                StatusCode = 404,
                Result = null
            };
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
        slot.CreatedTime = DateTime.UtcNow.AddHours(7.0);
        slot.Status = StaticOperationStatus.BaseEntity.Active;

        // This means if the DTO data is erroneous, the method will return a ResponseDTO with the error message
        if (ValidateSlotDtoError(slotDto) is { } errors)
        {
            return errors;
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


    public async Task<ResponseDto> UpdateSlot(ClaimsPrincipal user, UpdateSlotDto slotDto)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found",
                StatusCode = 404,
                Result = null
            };
        }
        var slotToUpdate = await _unitOfWork.Slot.GetAsync(Slot => Slot.SlotId == slotDto.SlotId);
        if (slotToUpdate is null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Slot not found",
                StatusCode = 404,
                Result = null
            };
        }
        if (ValidateSlotDtoError(slotDto) is { } errors)
        {
            return errors;
        }

        var updatedData = _autoMapperService.Map<UpdateSlotDto, Slot>(slotDto);
        updatedData.UpdatedBy = user.FindFirstValue("FullName");

        _unitOfWork.Slot.Update(slotToUpdate, updatedData);
        await _unitOfWork.SaveAsync();

        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Slot updated successfully",
            StatusCode = 200,
            Result = updatedData
        };
    }

    public async Task<ResponseDto> DeleteSlot(ClaimsPrincipal user, Guid slotId)
    {
        if (user.FindFirstValue(ClaimTypes.NameIdentifier) is null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "User not found",
                StatusCode = 404,
                Result = null
            };
        }

        var slotToDelete = await _unitOfWork.Slot.GetAsync(Slot => Slot.SlotId == slotId);
        if (slotToDelete is null)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Slot not found",
                StatusCode = 404,
                Result = null
            };
        }
        slotToDelete.Status = StaticOperationStatus.BaseEntity.Deleted;
        slotToDelete.UpdatedBy = user.FindFirstValue("FullName");

        var result = await _unitOfWork.SaveAsync();
        if (result == StaticOperationStatus.Database.Failure)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "Failed to delete slot",
                StatusCode = 500,
                Result = null
            };
        }
        return new ResponseDto
        {
            IsSuccess = true,
            Message = "Slot deleted successfully",
            StatusCode = 200,
            Result = slotToDelete
        };
    }

    /// <summary>
    /// Private method to validate the Slot DTO data
    /// </summary>
    /// <param name="slotDto">The DTO to create / update Slot</param>
    /// <returns string>A ResponseDTO with errors if the DTO data is erroneous, otherwise null</returns>
    private static ResponseDto? ValidateSlotDtoError<T>(T slotDto) where T : class
    {
        dynamic dto = slotDto;  // The DTO's properties will be accessible dynamically
        if (dto.EndTime <= dto.StartTime)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "EndTime must be later than StartTime.",
                StatusCode = 400,
                Result = null
            };
        }
        if (dto.EndTime - dto.StartTime < TimeSpan.FromMinutes(45))
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = "The duration between StartTime and EndTime must be at least 45 minutes.",
                StatusCode = 400,
                Result = null
            };
        }
        return null;
    }
}