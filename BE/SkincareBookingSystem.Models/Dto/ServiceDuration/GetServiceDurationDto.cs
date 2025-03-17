namespace SkincareBookingSystem.Models.Dto.ServiceDuration;

public class GetServiceDurationDto
{
    public Guid ServiceDurationId { get; set; }
    //public int DurationMinutes { get; set; }

    public string FormattedDuration { get; set; }
}