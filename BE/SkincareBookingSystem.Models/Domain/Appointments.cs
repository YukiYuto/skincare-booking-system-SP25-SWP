using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class Appointments : BaseEntity<string, string, string>
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; } = null!;
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")] public virtual Order Order { get; set; } = null!;
        public DateOnly AppointmentDate { get; set; }
        [StringLength(30)] public string AppointmentTime { get; set; } = string.Empty;
        [StringLength(200)] public string? Note { get; set; }
        public int DurationMinutes { get; set; }

        public virtual ICollection<TherapistSchedule> TherapistSchedules { get; set; } = new List<TherapistSchedule>();
    }
}
