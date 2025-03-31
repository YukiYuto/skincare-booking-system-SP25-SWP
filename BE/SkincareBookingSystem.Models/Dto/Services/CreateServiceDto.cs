
using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Dto.Services
{
    public class CreateServiceDto
    {
        public string ServiceName { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Range(10000, 1000000, ErrorMessage = "Service price must be between 10.000 and 1.000.000 VND.")]
        public double Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
