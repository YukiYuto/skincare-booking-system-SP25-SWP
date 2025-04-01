using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.Order
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public long OrderNumber { get; set; }
        public Guid CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedTime { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = [];

    }
}
