using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Appointment.Details
{
    public class ServiceInfoDto
    {
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int? ServiceDuration { get; set; }
        public string? ServicePrice { get; set; }
    }
}
