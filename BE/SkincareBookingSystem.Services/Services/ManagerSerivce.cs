using Microsoft.AspNetCore.Identity;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;
using SkincareBookingSystem.Models.Dto.LockUser;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class ManagerSerivce : IManagerSerivce
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public ManagerSerivce(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<ResponseDto> GetRevenueOrders(DateTime startDate, DateTime endDate, int pageNumber = 1,
        int pageSize = 10)
    {
        var orders = await _unitOfWork.Order.GetOrdersAsync(startDate, endDate);
        var totalOrders = orders.Count;
        var pagedOrders = orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling((double)orders.Count / pageSize);
        
        return new ResponseDto
        {
            Result = new
            {
                TotalOrders = totalOrders,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Orders = pagedOrders.Select(o => o.OrderId)
            },
            Message = "Revenue orders retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> GetRevenueProfit(DateTime startDate, DateTime endDate, int pageNumber = 1,
        int pageSize = 10)
    {
        var transactions = await _unitOfWork.Transaction.GetTransactionsAsync(startDate, endDate);
        var totalProfit = transactions.Sum(transaction => transaction.Amount);
        var pagedTransactions = transactions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling((double)transactions.Count / pageSize);

        return new ResponseDto
        {
            Result = new
            {
                TotalProfit = totalProfit,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Transactions = pagedTransactions
            },
            Message = "Revenue profit retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

    public async Task<ResponseDto> GetRevenueTransactions(DateTime startDate, DateTime endDate, int pageNumber = 1,
        int pageSize = 10)
    {
        var transactions = await _unitOfWork.Transaction.GetTransactionsAsync(startDate, endDate);
        var totalTransactions = transactions.Count;
        var pagedTransactions = transactions.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        var totalPages = (int)Math.Ceiling((double)transactions.Count / pageSize);

        return new ResponseDto
        {
            Result = new
            {
                TotalTransactions = totalTransactions,
                TotalPages = totalPages,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                Transactions = pagedTransactions
            },
            Message = "Revenue transactions retrieved successfully",
            IsSuccess = true,
            StatusCode = 200
        };
    }

        public async Task<ResponseDto> LockUser(LockUserDto lockUserDto)
    {
        if (string.IsNullOrEmpty(lockUserDto.UserId))
        {
            return new ResponseDto
            {
                Message = "User ID is required.",
                IsSuccess = false,
                StatusCode = 400
            };
        }

        var user = await _userManager.FindByIdAsync(lockUserDto.UserId);
        if (user == null)
        {
            return new ResponseDto
            {
                Message = $"User with ID '{lockUserDto.UserId}' not found.",
                IsSuccess = false,
                StatusCode = 404
            };
        }

        if (lockUserDto.LockoutEndDate.HasValue)
        {
            user.LockoutEnd = lockUserDto.LockoutEndDate.Value;
        }
        else
        {
            user.LockoutEnd = DateTimeOffset.MaxValue;
        }

        IdentityResult result = await _userManager.SetLockoutEndDateAsync(user, user.LockoutEnd);

        if (result.Succeeded)
        {
            return new ResponseDto
            {
                Message = "Lock user successfully",
                IsSuccess = true,
                StatusCode = 200,
                Result = new
                {
                    user.LockoutEnd,
                    lockUserDto.UserId
                } 
                    
            };
        }
        return new ResponseDto
        {
            Message = "Failed to lock user. Please check logs for details.",
            IsSuccess = false,
            StatusCode = 500
        };
    }

    public async Task<ResponseDto> UnlockUser(UnLockUserDto unLockUserDto)
    {
        var user = await _userManager.FindByIdAsync(unLockUserDto.UserId);

        if (user is null)
        {
            return new ResponseDto()
            {
                Message = "User was not found",
                IsSuccess = false,
                StatusCode = 404,
                Result = null
            };
        }

        user.LockoutEnd = null;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return new ResponseDto()
            {
                Message = "Unlock user was failed",
                IsSuccess = false,
                StatusCode = 400,
                Result = null
            };
        }

        return new ResponseDto()
        {
            Message = "Unlock user successfully",
            IsSuccess = true,
            StatusCode = 200,
            Result = unLockUserDto.UserId
        };
    }
}