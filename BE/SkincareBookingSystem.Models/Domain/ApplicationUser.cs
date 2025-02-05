using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)] public string? FullName { get; set; }
        public int Age { get; set; }
        [StringLength(50)] public string? Address { get; set; }
        [StringLength(200)] public string? ImageUrl { get; set; }
    }
}
