namespace SkincareBookingSystem.Models.Dto.Customer;

public class RecommendationResponseDto
{
    public List<ServiceRecommenedDto> Services { get; set; } = null!;
    public List<ServiceComboRecommenedDto> ServiceCombos { get; set; } = null!;
}