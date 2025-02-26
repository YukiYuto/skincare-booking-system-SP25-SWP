using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface ISlotService 
    {
        Task<ResponseDto> GetSlotById(Guid id);
        Task<ResponseDto> GetAllSlots();
        Task<ResponseDto> CreateSlot(ClaimsPrincipal User, CreateSlotDto createSlotDto);
        Task<ResponseDto> UpdateSlot(ClaimsPrincipal User, UpdateSlotDto updateSlotDto);
        Task<ResponseDto> DeleteSlot(Guid id);
    }
}
