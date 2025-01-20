using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(30)] public string? FullName { get; set; }
        public int Age { get; set; }
        [StringLength(50)] public string? Address { get; set; }

    }
}
