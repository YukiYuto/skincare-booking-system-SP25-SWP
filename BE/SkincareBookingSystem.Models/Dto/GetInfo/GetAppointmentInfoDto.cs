using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.GetInfo
{
    public class GetAppointmentInfoDto
    {
        public string? TherapistName { get; set; }
        public string? ServiceName { get; set; }
        public DateTime? Date { get; set; }
    }
}
