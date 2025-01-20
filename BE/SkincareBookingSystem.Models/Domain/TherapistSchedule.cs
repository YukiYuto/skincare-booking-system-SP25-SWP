using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class TherapistSchedule : BaseEntity<string, string, string>
    {
        [Key]
        public Guid TherapistScheduleID { get; set; }
        public Guid AppointmentID { get; set; }

        public Guid SlotID { get; set; }
        [ForeignKey("SlotID")]
        public virtual Slot Slot { get; set; }
    }
}
