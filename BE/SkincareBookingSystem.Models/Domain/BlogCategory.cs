using System.ComponentModel.DataAnnotations;

namespace SkincareBookingSystem.Models.Domain
{
    public class BlogCategory : BaseEntity<string, string, string>
    {
        [Key]
        public Guid BlogCategoryId { get; set; }
        [StringLength(30)] public string Name { get; set; } = null!;
        [StringLength(100)] public string Description { get; set; } = null!;
        
        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
