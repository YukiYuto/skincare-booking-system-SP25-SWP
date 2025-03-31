using SkincareBookingSystem.Models.Dto.LockUser;
using SkincareBookingSystem.Models.Dto.Response;

namespace SkincareBookingSystem.Services.IServices;

public interface IManagerSerivce
{
    Task<ResponseDto> GetRevenueOrders(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto> GetRevenueProfit(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto> GetRevenueTransactions(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 10);
    Task<ResponseDto> LockUser (LockUserDto lockUserDto);
    Task<ResponseDto> UnlockUser (UnLockUserDto unLockUserDto);
}