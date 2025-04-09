namespace SkincareBookingSystem.Models.Dto.Appointment;

public class AppointmentResponseDto
{
    public Guid AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public DateOnly AppointmentDate { get; set; }
    public string AppointmentTime { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string TherapistName { get; set; } = null!;
}