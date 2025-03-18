using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TherapistSchedules
{
    public class GetTherapistScheduleDto
    {
        public Guid? AppointmentId { get; set; }
        public Guid? TherapistId { get; set; }
        public Guid? SlotId { get; set; }
    }
}
