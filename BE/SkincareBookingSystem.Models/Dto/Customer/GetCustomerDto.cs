using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Customer
{
    public class GetCustomerDto
    {
        public string CustomerId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public string? Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? ImageUrl { get; set; } = null!;
        public string? SkinProfileId { get; set; }
    }
}
