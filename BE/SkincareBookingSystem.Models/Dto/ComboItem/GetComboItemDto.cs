namespace SkincareBookingSystem.Models.Dto.ComboItem;

public class GetComboItemDto
{
    public Guid ServiceComboId { get; set; }
    public List<ServicePriorityDto> ServicePriorityDtos { get; set; }
}