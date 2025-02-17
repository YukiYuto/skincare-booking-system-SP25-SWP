namespace SkincareBookingSystem.Models.Dto.Slot;

public class UpdateSlotDto
{
    public Guid SlotId { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Guid? TherapistScheduleId { get; set; }
}