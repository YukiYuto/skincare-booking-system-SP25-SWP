using System.Security.Claims;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Slot;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class SlotService : ISlotService
{
    private readonly IUnitOfWork _unitOfWork;

    public SlotService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public Task<ResponseDto> CreateSlot(ClaimsPrincipal user, CreateSlotDto slotDto)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetAllSlots()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseDto> GetSlot(Guid slotId)
    {
        throw new NotImplementedException();
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