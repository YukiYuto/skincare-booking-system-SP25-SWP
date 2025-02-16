using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Orders
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public double TotalPrice { get; set; }
        public string? CreatedDate { get; set; } 
        public string? CreatedBy { get; set; }
    }
}
