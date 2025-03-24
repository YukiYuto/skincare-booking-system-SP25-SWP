using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Blog
{
    public class BlogDetailDto
    {
        public Guid? BlogId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? BlogCategoryId { get; set; }
        public string? BlogCategoryName { get; set; }
        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? Tags { get; set; }
        public string? ImageUrl { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string? Status { get; set; }
    }
}
