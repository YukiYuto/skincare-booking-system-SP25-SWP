using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class SkinTherapist
    {
        [Key]
        public Guid SkinTherapistId { get; set; }

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [StringLength(20)] public string Experience { get; set; } = null!;

        public Guid? TherapistScheduleId {  get; set; }
        [ForeignKey("TherapistScheduleId")]
        public virtual TherapistSchedule TherapistSchedule { get; set; } = null!;
    }
}
