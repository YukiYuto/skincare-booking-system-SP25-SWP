using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Booking.Appointment
{
    public class AutoAssignmentDto
    {
        public Guid ServiceTypeId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public Guid SlotId { get; set; }
    }
}
