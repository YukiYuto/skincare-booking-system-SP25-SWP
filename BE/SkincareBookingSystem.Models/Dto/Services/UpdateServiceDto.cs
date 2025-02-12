using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Services
{
    public class UpdateServiceDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? ServiceTypeId { get; set; }
    }
}
