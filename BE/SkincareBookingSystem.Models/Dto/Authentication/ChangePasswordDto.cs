using SkincareBookingSystem.Utilities.ValidationAttribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Authentication
{
    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string OldPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Password]
        public string NewPassword { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [ConfirmPassword("NewPassword")]
        public string ConfirmNewPassword { get; set; } = null!;
        
        public string UserId { get; set; } = null!;
    }
}
