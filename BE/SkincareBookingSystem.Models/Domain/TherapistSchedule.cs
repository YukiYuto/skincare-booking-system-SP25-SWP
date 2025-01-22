using System.ComponentModel.DataAnnotations;


namespace SkincareBookingSystem.Models.Domain
{
    public class TherapistSchedule : BaseEntity<string, string, string>
    {
        [Key]
        public Guid TherapistScheduleId { get; set; }
        public Guid AppointmentId { get; set; }
        
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
