namespace SkincareBookingSystem.Models.Dto.Booking.Appointment;

public class CancelAppointmentDto
{
    public Guid AppointmentId { get; set; }
    public string Reason { get; set; } = null!;
}