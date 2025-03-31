using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Services
{
    public class UpdateServiceDto
    {
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        [Range(10000, 1000000, ErrorMessage = "Service price must be between 10.000 and 1.000.000 VND.")]
        public double? Price { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string Status { get; set; } = "Active";
    }
}
