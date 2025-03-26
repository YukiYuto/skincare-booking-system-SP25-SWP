using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Appointment.Details
{
    public class AppointmentDetailsDto
    {
        public CustomerInfoDto CustomerInfo { get; set; } = null!;
        public ServiceInfoDto ServiceInfo { get; set; } = null!;
        public TherapistInfoDto TherapistInfo { get; set; } = null!;
        public string? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? CreatedTime { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
    }
}
