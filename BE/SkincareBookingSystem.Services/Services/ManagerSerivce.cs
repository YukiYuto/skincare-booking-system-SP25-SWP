using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Dto.Response;
using SkincareBookingSystem.Services.IServices;

namespace SkincareBookingSystem.Services.Services;

public class ManagerSerivce : IManagerSerivce
{
    private readonly IUnitOfWork _unitOfWork;

    public ManagerSerivce(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
}