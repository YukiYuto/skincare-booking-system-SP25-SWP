using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.SkinTherapist
{
    public class GetSkinTherapistDto
    {
        public string SkinTherapistId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string? ImageUrl { get; set; } = null!;
        public string? Experience { get; set; } = null!;
    }
}
