namespace SkincareBookingSystem.Models.Dto.ServiceCombo;

public class ServiceComboDto
{
    public Guid ServiceComboId { get; set; }
    public string ComboName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int NumberOfService { get; set; }
    public string? ImageUrl { get; set; }
    public string Status { get; set; } = null!;
    public List<ServiceDto> Services { get; set; } = null!;
}