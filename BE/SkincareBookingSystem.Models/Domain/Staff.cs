using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public class Staff 
{
    [Key]
    public Guid StaffId { get; set; }
    public string UserId { get; set; } = null!;
    [ForeignKey("UserId")]public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    [StringLength(8)]public string StaffCode { get; set; } = null!;
}