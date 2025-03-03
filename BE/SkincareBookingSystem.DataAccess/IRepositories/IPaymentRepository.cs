using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment> GetPaymentByOrderNumber(long orderNumber);
    Task<Payment> GetPaymentTransactionIdByOrderNumber(Guid paymentTransactionId);
    void Update(Payment payment);
}