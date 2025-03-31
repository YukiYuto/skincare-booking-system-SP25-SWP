namespace SkincareBookingSystem.Models.Dto.Customer;

public class ServiceComboRecommenedDto
{
    public Guid ServiceComboId { get; set; }
    public string ServiceComboName { get; set; } = null!;
    public string ServiceComboImage { get; set; } = null!;
    public double ServiceComboPrice { get; set; }
}