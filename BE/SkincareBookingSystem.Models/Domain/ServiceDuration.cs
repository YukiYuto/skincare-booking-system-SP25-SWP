using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Domain;

public class ServiceDuration
{
    [Key] public Guid ServiceDurationId { get; set; }

    public int DurationMinutes { get; set; }

    public virtual ICollection<DurationItem> DurationItems { get; set; } = new List<DurationItem>();

    public string GetFormattedDuration()
    {
        var hours = DurationMinutes / 60;
        var minutes = DurationMinutes % 60;
        return $"{hours} hour(s) {minutes} minute(s)";
    }
}