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
        
        
        public virtual ICollection<SkinServiceType> SkinServiceTypes { get; set; } = new List<SkinServiceType>();
        
        public virtual ICollection<TherapistServiceType> TherapistServiceTypes { get; set; } = new List<TherapistServiceType>();
        public virtual ICollection<TherapistSchedule> TherapistSchedules { get; set; } = new List<TherapistSchedule>();
    }
}
