using SkincareBookingSystem.Models.Dto.Services;

namespace SkincareBookingSystem.Models.Dto.TypeItem;

public class GetTypeItemsDto
{
    public Guid ServiceTypeId { get; set; }
    public string ServiceTypeName { get; set; } = null!;
    public List<ServiceDto> Services { get; set; } = new();
}