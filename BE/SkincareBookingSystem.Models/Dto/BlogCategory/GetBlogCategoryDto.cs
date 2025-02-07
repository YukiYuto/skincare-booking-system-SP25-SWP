using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.BlogCategory
{
    public class GetBlogCategoryDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedTime { get; set; }
        public string? Status { get; set; }
    }
}
