namespace SkincareBookingSystem.Models.Dto.Customer;

public class ServiceRecommenedDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public string ServiceImage { get; set; } = null!;
    public double ServicePrice { get; set; }
}