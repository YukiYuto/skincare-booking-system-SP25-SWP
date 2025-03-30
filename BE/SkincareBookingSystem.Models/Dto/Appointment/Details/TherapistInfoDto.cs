using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Appointment.Details
{
    public class TherapistInfoDto
    {
        public Guid TherapistId { get; set; }
        public string? TherapistName { get; set; }
        public string? TherapistAge { get; set; }
        public string? TherapistAvatarUrl { get; set; }
        public string? TherapistExperience { get; set; }
        public string? TherapistGender { get; set; }
    }
}
