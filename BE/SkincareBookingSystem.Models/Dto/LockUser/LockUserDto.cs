using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Dto.LockUser;

public class LockUserDto
{
    [Required]
    public string UserId { get; set; } = null!;
    public DateTimeOffset? LockoutEndDate { get; set; }
}