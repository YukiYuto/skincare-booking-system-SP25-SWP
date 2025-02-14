using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slot;

namespace SkincareBookingSystem.Services.IServices;

public interface ISlotService
{
    Task<ResponseDto> CreateSlot(ClaimsPrincipal user, CreateSlotDto slotDto);
    Task<ResponseDto> GetAllSlots();
    Task<ResponseDto> GetSlot(Guid slotId);
    Task<ResponseDto> UpdateSlot(ClaimsPrincipal user, UpdateSlotDto slotDto);
    Task<ResponseDto> DeleteSlot(ClaimsPrincipal user, Guid slotId);
}