using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.ComboItem;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IComboItemService
{
    Task<ResponseDto> CreateComboItem(ClaimsPrincipal user, CreateComboItemDto createComboItemDto);
    Task<ResponseDto> GetAllComboItem
    (
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterOn = null,
        string? filterQuery = null,
        string? sortBy = null
    );
}