using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<List<Transaction>> GetTransactionsAsync(DateTime startDate, DateTime endDate);
}