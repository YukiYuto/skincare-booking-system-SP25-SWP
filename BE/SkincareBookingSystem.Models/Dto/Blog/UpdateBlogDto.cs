using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Blog
{
    public class UpdateBlogDto
    {
        public Guid? BlogId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Tags { get; set; }
        public string? ImageUrl { get; set; }
        public string? UserId { get; set; }
        public Guid? BlogCategoryId { get; set; }

    }
}
