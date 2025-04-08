namespace SkincareBookingSystem.Models.Dto.SkinTherapist;

public class CreateTherapistAdviceDto
{
    public Guid TherapistScheduleId { get; set; }
    public Guid CustomerId { get; set; }
    public string AdviceContent { get; set; } = null!;
}