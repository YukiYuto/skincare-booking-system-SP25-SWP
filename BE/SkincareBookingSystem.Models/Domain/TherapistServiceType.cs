using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public class TherapistServiceType
{
    public Guid TherapistId { get; set; }
    [ForeignKey("TherapistId")] 
    public virtual SkinTherapist SkinTherapist { get; set; } = null!;
    
    public Guid ServiceTypeId { get; set; }
    [ForeignKey("ServiceTypeId")]
    public virtual ServiceType ServiceType { get; set; } = null!;
    
}