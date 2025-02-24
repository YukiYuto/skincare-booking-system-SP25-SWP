using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Appointment
{
    public class CreateAppointmentDto
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = null!;
    }
}
