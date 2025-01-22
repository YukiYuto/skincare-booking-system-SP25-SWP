using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class DurationItem
    {
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Services Services { get; set; } = null!;

        public Guid ServiceDurationId { get; set; }
        [ForeignKey("ServiceDurationId")]
        public virtual ServiceDuration ServiceDuration { get; set; } = null!;
    }
}
