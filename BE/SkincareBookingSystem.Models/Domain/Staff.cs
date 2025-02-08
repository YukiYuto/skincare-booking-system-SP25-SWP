using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SkincareBookingSystem.Models.Domain;

public class Staff 
{
    [Key]
    public Guid StaffId { get; set; }
    public string UserId { get; set; } = null!;
    [ForeignKey("UserId")]public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public string StaffCode { get; set; } = null!;
}