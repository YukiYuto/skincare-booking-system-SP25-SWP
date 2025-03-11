using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.TypeItem;

namespace SkincareBookingSystem.Services.IServices;

public interface ITypeItemService
{
    Task<ResponseDto> CreateTypeItem(ClaimsPrincipal user, CreateTypeItemDto createTypeItemDto);

    Task<ResponseDto> GetAllTypeItem
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
}