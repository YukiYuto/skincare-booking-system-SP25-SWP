namespace SkincareBookingSystem.Models.Dto.TypeItem;

public class CreateTypeItemDto 
{
    public Guid ServiceId { get; set; }
    public List<Guid> ServiceTypeIdList { get; set; }
}