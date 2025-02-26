using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.BookingSchedule
{
    public class UpdateBookingScheduleDto
    {
        public Guid BookingScheduleId { get; set; }
        public Guid? SkinTherapistId { get; set; }
        public Guid? SlotId { get; set; }
    }
}
