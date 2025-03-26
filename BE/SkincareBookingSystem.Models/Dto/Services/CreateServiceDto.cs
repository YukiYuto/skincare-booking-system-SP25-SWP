
using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Dto.Services
{
    public class CreateServiceDto
    {
        public string ServiceName { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Range(10000, double.MaxValue, ErrorMessage = "Giá dịch vụ phải từ 10,000 trở lên.")]
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
