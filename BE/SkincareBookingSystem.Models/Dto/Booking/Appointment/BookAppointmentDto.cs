namespace SkincareBookingSystem.Models.Dto.Booking.Appointment;

namespace SkincareBookingSystem.Models.Dto.Booking.Appointment
{
    public class BookAppointmentDto
    {
        public Guid TherapistId { get; set; } = Guid.Empty;
        public Guid SlotId { get; set; } = Guid.Empty;
        public Guid? CustomerId { get; set; } = null!;
        public DateOnly AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? Note { get; set; }
        public long OrderNumber { get; set; }
    }
}
