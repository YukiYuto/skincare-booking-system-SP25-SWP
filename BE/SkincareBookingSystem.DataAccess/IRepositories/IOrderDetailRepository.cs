using System;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.DataAccess.IRepositories;

public interface IOrderDetailRepository : IRepository<OrderDetail>
{
    void Update(OrderDetail target, OrderDetail source);
    Task<(List<Services>, List<ServiceCombo>)> GetServicesAndCombosByOrderIdAsync(Guid orderId);
}
