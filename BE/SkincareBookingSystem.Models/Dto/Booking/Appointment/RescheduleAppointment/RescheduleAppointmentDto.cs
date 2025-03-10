using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.Appointment.RescheduleAppointment
{
    public class RescheduleAppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid NewSlotId { get; set; }
        public string? Reason { get; set; }
    }
}
