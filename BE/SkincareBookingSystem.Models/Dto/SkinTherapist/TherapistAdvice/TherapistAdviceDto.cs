namespace SkincareBookingSystem.Models.Dto.SkinTherapist;

public class TherapistAdviceDto
{
    public Guid AdviceId { get; set; }
    public string AdviceContent { get; set; } = null!;
    public Guid TherapistScheduleId { get; set; }
    public Guid CustomerId { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}