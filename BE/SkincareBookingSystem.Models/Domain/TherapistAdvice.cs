using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public class TherapistAdvice : BaseEntity<string, string, string>
{
    [Key]
    public Guid AdviceId { get; set; }

    public Guid TherapistScheduleId { get; set; }
    [ForeignKey("TherapistScheduleId")]
    public virtual TherapistSchedule TherapistSchedule { get; set; } = null!;

    public Guid CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; } = null!;

    [Required]
    [StringLength(1000)]
    public string AdviceContent { get; set; } = null!;
}