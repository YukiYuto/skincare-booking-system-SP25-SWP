using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.OrderDetails
{
    public class CreateOrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid? ServiceComboId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Description { get; set; } = null!;
    }
}
