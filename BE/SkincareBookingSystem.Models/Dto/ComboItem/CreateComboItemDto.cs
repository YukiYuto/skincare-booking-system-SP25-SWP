namespace SkincareBookingSystem.Models.Dto.ComboItem;

public class CreateComboItemDto
{
    public Guid ServiceComboId { get; set; }
    public List<ServicePriorityDto> ServicePriorityDtos { get; set; }
}