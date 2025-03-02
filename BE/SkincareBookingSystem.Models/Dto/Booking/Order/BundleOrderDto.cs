using SkincareBookingSystem.Models.Dto.OrderDetails;
using SkincareBookingSystem.Models.Dto.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.Order
{
    public class BundleOrderDto
    {
        public CreateOrderDto Order { get; set; } = null!;
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = null!;
    }
}
