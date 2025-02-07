using SkincareBookingSystem.Models.Domain;
using System.Runtime.InteropServices;

namespace SkincareBookingSystem.Models.Dto.BlogCategory
{
    public class CreateBlogCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public string? Status { get; set; }
        public string? CreatedTime { get; set; }
    }
}
