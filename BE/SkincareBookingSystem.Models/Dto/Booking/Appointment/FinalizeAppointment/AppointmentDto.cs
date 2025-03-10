using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment
{
    public class AppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        public string? Note { get; set; }
    }
}
