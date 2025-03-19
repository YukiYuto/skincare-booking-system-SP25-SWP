using SkincareBookingSystem.DataAccess.DBContext;
using SkincareBookingSystem.DataAccess.IRepositories;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.Repositories;

public class OrderServiceTrackingRepository : Repository<OrderServiceTracking>, IOrderServiceTrackingRepository
{
    private readonly ApplicationDbContext _context;
    
    public OrderServiceTrackingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderServiceTracking orderServiceTracking)
    {
        _context.OrderServiceTracking.Update(orderServiceTracking);
    }
}