using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TypeItem;

namespace SkincareBookingSystem.Services.IServices;

public interface ITypeItemService
{
    Task<ResponseDto> CreateTypeItem(ClaimsPrincipal user, CreateTypeItemDto createTypeItemDto);
    Task<ResponseDto> RemoveTypeItem(ClaimsPrincipal user, CreateTypeItemDto removeTypeItemDto);
}