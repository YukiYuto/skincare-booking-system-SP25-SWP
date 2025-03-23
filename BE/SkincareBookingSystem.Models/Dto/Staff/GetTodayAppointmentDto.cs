using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Models.Dto.Staff;

public class GetTodayAppointmentDto
{
    public string Therapist { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public ScheduleStatus? Status { get; set; }
    public Guid AppointmentId { get; set; }
    public Guid CustomerId { get; set; }
} 