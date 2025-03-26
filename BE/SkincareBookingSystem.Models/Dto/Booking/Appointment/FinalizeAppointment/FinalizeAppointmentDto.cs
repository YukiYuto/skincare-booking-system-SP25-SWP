using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkincareBookingSystem.Models.Dto.Booking.Appointment.FinalizeAppointment;
using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Models.Dto.Booking.Appointment.NewFolder
{
    public class FinalizeAppointmentDto
    {
        public AppointmentDto Appointment { get; set; }
        public TherapistSchedule TherapistSchedule { get; set; }
    }
}
