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
        [Range(10000, double.MaxValue, ErrorMessage = "Giá dịch vụ phải từ 10,000 trở lên.")]
        public double? Price { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public string Status { get; set; } = "Active";
    }
}
