using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class Slot : BaseEntity<string, string, string>
    {
        [Key]
        public Guid SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Guid? TherapistScheduleId { get; set; }
        [ForeignKey("TherapistScheduleId")]
        public virtual TherapistSchedule TherapistSchedule { get; set; } = null!;
    }
}
