namespace SkincareBookingSystem.Models.Dto.Services;

public class GetAllServicesDto
{
    public Guid ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = null!;
    public Guid ServiceTypeId { get; set; }
}