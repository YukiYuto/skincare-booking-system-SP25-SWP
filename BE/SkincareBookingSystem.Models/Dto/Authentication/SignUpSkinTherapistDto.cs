using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SkincareBookingSystem.Utilities.ValidationAttribute;

namespace SkincareBookingSystem.Models.Dto.Authentication;

public class SignUpSkinTherapistDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [ConfirmPassword("Password")]
    [NotMapped]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [DataType(DataType.PhoneNumber)]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [Required] public string FullName { get; set; } = null!;

    [Required] public string Address { get; set; } = null!;

    [Required] public DateTime BirthDate { get; set; }
    [Required] public string Gender { get; set; } = null!;

    [Required] public string Experience { get; set; } = null!;

    public Guid? TherapistScheduleId { get; set; } = null;
}