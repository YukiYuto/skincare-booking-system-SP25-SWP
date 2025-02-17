using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Orders
{
    public class UpdateOrderDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public double TotalPrice { get; set; }
    }
}
