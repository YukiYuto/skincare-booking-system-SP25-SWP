namespace SkincareBookingSystem.Models.Dto.Staff;

public class CheckInDto
{
    public Guid CustomerId { get; set; }
    public Guid AppointmentId { get; set; }
}