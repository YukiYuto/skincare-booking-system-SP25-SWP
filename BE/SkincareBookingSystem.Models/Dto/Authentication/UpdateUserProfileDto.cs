using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Authentication
{
    public class UpdateUserProfileDto
    {
        public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? ImageUrl { get; set; }
    }
}