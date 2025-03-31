using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.OrderDetails
{
    public class GetOrderDetailDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }

    }
}
