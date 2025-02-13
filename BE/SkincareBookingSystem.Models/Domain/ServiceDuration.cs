using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public class ServiceDuration
{
    [Key]
    public Guid ServiceDurationId { get; set; }
    public int DurationMinutes { get; set; }  
    
    public string GetFormattedDuration()  
    {  
        int hours = DurationMinutes / 60; // Tính số giờ  
        int minutes = DurationMinutes % 60; // Tính số phút  
        return $"{hours} giờ {minutes} phút";  
    }  
    
    public virtual ICollection<DurationItem> DurationItems { get; set; } = new List<DurationItem>();
}