using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.BookingSchedule
{
    public class UpdateTherapistScheduleDto
    {
        public Guid TherapistScheduleId { get; set; }
        public Guid? TherapistId { get; set; }
        public Guid? AppointmentId { get; set; }
        public Guid? SlotId { get; set; }
    }
}
