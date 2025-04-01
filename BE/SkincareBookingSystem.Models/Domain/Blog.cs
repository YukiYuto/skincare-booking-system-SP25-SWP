using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class Blog : BaseEntity<string, string, string>
    {
        [Key] 
        public Guid BlogId { get; set; }
        [StringLength(50)] public string Title { get; set; } = null!;
        [StringLength(500)] public string Content { get; set; } = null!;

        public Guid BlogCategoryId { get; set; }
        [ForeignKey("BlogCategoryId")]
        public virtual BlogCategory BlogCategory { get; set; } = null!;
        
        public string AuthorId { get; set; } = null!;
        [ForeignKey("AuthorId")] 
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [StringLength(30)] public string Tags { get; set; } = null!;
        [StringLength(500)]public string? ImageUrl { get; set; }

    }
}
