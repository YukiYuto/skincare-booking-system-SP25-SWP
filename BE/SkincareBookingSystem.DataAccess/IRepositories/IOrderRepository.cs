using SkincareBookingSystem.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.DataAccess.IRepositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderByOrderNumber(long orderNumber);
        Task<long> GenerateUniqueNumberAsync();
        Task<Order?> GetLatestOrderByCustomerIdAsync(Guid customerId);
        void Update(Order target, Order source);
        Task<List<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate);
    }
}
