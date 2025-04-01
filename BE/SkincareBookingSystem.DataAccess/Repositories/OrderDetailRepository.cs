using Microsoft.EntityFrameworkCore;
using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail target, OrderDetail source)
        {
            _context.Set<OrderDetail>().Attach(target);
            _context.Entry(target).State = EntityState.Modified;
            _context.Entry(target).CurrentValues.SetValues(source);
        }

        public async Task<(List<Services>, List<ServiceCombo>)> GetServicesAndCombosByOrderIdAsync(Guid orderId)
        {
            var orderDetails = await _context.OrderDetail
                .AsNoTracking()
                .Where(od => od.OrderId == orderId)
                .ToListAsync();

            var serviceIds = new HashSet<Guid>(
                orderDetails
                    .Where(od => od.ServiceId != null)
                    .Select(od => od.ServiceId.Value)
            );

            var serviceComboIds = orderDetails
                .Where(od => od.ServiceComboId != null)
                .Select(od => od.ServiceComboId.Value)
                .Distinct()
                .ToList();

            var services = await _context.Services
                .Where(s => serviceIds.Contains(s.ServiceId))
                .ToListAsync();

            var serviceCombos = await _context.ServiceCombo
                .Where(sc => serviceComboIds.Contains(sc.ServiceComboId))
                .ToListAsync();

            return (services, serviceCombos);
        }
    }
}