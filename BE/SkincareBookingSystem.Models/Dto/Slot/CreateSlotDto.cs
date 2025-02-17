namespace SkincareBookingSystem.Models.Dto.Slot;

public class CreateSlotDto
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid? TherapistScheduleId { get; set; }
}