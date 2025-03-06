using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class TherapistSchedule : BaseEntity<string, string, string>
    {
        [Key]
        public Guid TherapistScheduleId { get; set; }

        public Guid AppointmentId { get; set; }
        [ForeignKey("AppointmentId")] public virtual Appointments Appointment { get; set; }

        public Guid SlotId { get; set; }
        [ForeignKey("SlotId")] public virtual Slot Slot { get; set; }

        public Guid TherapistId { get; set; }
        [ForeignKey("TherapistId")] public virtual SkinTherapist SkinTherapist { get; set; }
        [StringLength(255)] public string? Reason { get; set; }
    }
}
