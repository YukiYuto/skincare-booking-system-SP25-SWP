using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment> GetPaymentByOrderNumber(long orderNumber);

    Task<(List<Payment> Payments, int TotalPayments)> GetPaymentsAsync
    (
        int pageNumber,
        int pageSize,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        Guid? customerId = null
    );

    void Update(Payment payment);
}