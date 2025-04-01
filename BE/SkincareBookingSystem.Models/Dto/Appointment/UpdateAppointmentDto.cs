using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Appointment
{
    public class UpdateAppointmentDto
    {
        public Guid AppointmentId { get; set; }
        public Guid? OrderId { get; set; }
        public DateOnly? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
    }
}
