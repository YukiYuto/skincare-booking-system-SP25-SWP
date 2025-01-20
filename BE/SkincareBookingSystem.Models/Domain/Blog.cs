using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Blog : BaseEntity<string, string, string>
    {
        [Key] 
        public Guid BlogID { get; set; }
        [StringLength(30)] public string Title { get; set; } = null!;
        [StringLength(100)] public string Content { get; set; } = null!;

        public Guid BlogCategoryID { get; set; }
        [ForeignKey("BlogCategoryID")]
        public virtual BlogCategory BlogCategory { get; set; } = null!;

        [StringLength(30)] public string Tags { get; set; } = null!;
        public string? ImageUrl { get; set; }

    }
}
