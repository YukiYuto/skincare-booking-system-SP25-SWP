using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public Task<Order> GetOrderByOrderNumber(long orderNumber)
        {
            return _context.Order.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<long> GenerateUniqueNumberAsync()
        {
            var maxOrderNumber = await _context.Order.MaxAsync(o => (long?)o.OrderNumber) ?? 0;
            return maxOrderNumber + 1;
        }

        public async Task<Order?> GetLatestOrderByCustomerIdAsync(Guid customerId)
        {
            return await _context.Order
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderNumber)
                .FirstOrDefaultAsync();
        }

        public void Update(Order target, Order source)
        {
            _context.Set<Order>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }

        public async Task<List<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate)
        {
            var orders = _context.Order
                .Where(o => o.CreatedTime >= startDate.ToUniversalTime() 
                            && o.CreatedTime <= endDate.ToUniversalTime())
                .ToListAsync();
            
            return await orders;
        }
    }
}