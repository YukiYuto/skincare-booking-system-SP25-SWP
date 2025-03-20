using System.Security.Claims;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Models.Dto.Staff;

namespace SkincareBookingSystem.Services.IServices;

public interface IStaffService
{
    Task<ResponseDto> GetCustomerByInfor(
        ClaimsPrincipal User,
        int pageNumber = 1,
        int pageSize = 10,
        string? filterQuery = null
    );
    
    Task<ResponseDto> CheckInCustomer(ClaimsPrincipal User, CheckInDto checkInDto);
    Task<ResponseDto> CheckOutCustomer(ClaimsPrincipal User, CheckInDto checkOutDto);
}