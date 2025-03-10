namespace SkincareBookingSystem.Models.Dto.ServiceCombo;

public class CreateServiceComboDto
{
    public string ComboName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double Price { get; set; }
    public int NumberOfService { get; set; }
    public string? ImageUrl { get; set; }
}