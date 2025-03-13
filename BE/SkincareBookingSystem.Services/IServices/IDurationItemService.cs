using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.DurationItem;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IDurationItemService
{
    Task<ResponseDto> AddDurationItemAsync(ClaimsPrincipal User, CreateDurationItemDto durationItemDto);
    Task<ResponseDto> DeleteDurationItemAsync(Guid id);
}