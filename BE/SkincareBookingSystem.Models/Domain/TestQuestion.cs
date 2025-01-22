using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class TestQuestion
    {
        [Key]
        public Guid TestQuestionId { get; set; }

        public Guid SkinTestId { get; set; }
        [ForeignKey("SkinTestId")]
        public virtual SkinTest SkinTest { get; set; } = null!;

        [StringLength(30)] public string Content { get; set; } = null!;
        [StringLength(30)] public string Type { get; set; } = null!;
    }
}
