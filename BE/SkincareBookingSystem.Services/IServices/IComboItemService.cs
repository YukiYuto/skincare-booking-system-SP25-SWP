using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IComboItemService
{
    Task<ResponseDto> CreateComboItem(ClaimsPrincipal user, CreateComboItemDto createComboItemDto);
    
}