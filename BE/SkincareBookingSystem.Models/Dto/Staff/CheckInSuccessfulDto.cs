namespace SkincareBookingSystem.Models.Dto.Staff;

public class CheckInSuccessfulDto
{
    public Guid CustomerId { get; set; }
    public Guid AppointmentId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CheckInTime { get; set; }
}