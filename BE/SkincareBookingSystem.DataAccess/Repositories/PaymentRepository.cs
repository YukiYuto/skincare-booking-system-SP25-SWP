using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Payment> GetPaymentByOrderNumber(long orderNumber)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);
    }

    public async Task<(List<Payment> Payments, int TotalPayments)> GetPaymentsAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        Guid? customerId = null
    )
    {
        var query = _context.Payments.AsQueryable();
        
        if (customerId.HasValue)
        {
            query = query.Where(p => p.Orders.CustomerId == customerId.Value);
        }
        
        //Query
        if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
        {
            query = filterOn.ToLower() switch
            {
                "status" => Enum.TryParse<PaymentStatus>(filterQuery, true, out var status)
                    ? query.Where(p => p.Status == status)
                    : query,
                "amount" => int.TryParse(filterQuery, out int amount)
                    ? query.Where(p => p.Amount == amount)
                    : query,
                _ => query
            };
        }
        
        // Sum: COUNT
        int totalPayments = await query.CountAsync();
        
        // Sort
        query = sortBy?.ToLower() switch
        {
            "amount" => query.OrderBy(p => p.Amount),
            "amount_desc" => query.OrderByDescending(p => p.Amount),
            "status" => query.OrderBy(p => p.Status),
            "createdat" => query.OrderBy(p => p.CreatedAt),
            "createdat_desc" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };
        

        var payments = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (payments, totalPayments);
    }

    public void Update(Payment payment)
    {
        _context.Payments.Update(payment);
    }
}