using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.GetCustomerInfo
{
    public class GetCustomerInfoDto
    {
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; } = null!;
    }
}
