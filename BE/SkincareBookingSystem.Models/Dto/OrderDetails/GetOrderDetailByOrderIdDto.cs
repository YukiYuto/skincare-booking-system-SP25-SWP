using SkincareBookingSystem.Models.Dto.Booking.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.OrderDetails
{
    public class GetOrderDetailByOrderIdDto
    {
        public Guid OrderId { get; set; }
        public List<GetOrderDetailDto>? OrderDetails { get; set; }

    }
}
