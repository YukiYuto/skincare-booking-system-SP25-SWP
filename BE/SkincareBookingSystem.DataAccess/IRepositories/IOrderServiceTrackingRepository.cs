using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IOrderServiceTrackingRepository : IRepository<OrderServiceTracking>
{
    void Update(OrderServiceTracking orderServiceTracking);
}