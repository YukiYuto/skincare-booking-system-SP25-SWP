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

    public async Task<Payment> GetPaymentTransactionIdByOrderNumber(Guid paymentTransactionId)
    {
        return await _context.Payments.FirstOrDefaultAsync(x => x.PaymentTransactionId == paymentTransactionId);
    }

    public  void Update(Payment payment)
    {
        _context.Payments.Update(payment);
    }
}