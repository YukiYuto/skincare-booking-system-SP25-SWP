using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.SkinTherapist
{
    public class PreviewTherapistDto
    {
        public Guid TherapistId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string ImageUrl { get; set; }
        public string Experience { get; set; }
    }
}
