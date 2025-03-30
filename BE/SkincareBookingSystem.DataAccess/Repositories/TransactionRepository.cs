using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }


    public async Task<List<Transaction>> GetTransactionsAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Transactions
            .Where(t => t.TransactionDateTime >= startDate.ToUniversalTime() &&
                        t.TransactionDateTime <= endDate.ToUniversalTime())
            .ToListAsync();
    }
}