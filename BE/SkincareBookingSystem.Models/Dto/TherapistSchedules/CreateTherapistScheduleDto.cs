using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.BookingSchedule
{
    public class CreateTherapistScheduleDto
    {
        public Guid SkinTherapistId { get; set; }
        public Guid SlotId { get; set; }
    }
}
