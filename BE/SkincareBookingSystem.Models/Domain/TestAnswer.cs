using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class TestAnswer : BaseEntity<string,string,string>
    {
        [Key]
        public Guid TestAnswerId { get; set; }

        public Guid TestQuestionId { get; set; }
        [ForeignKey("TestQuestionId")]
        public virtual TestQuestion TestQuestion { get; set; } = null!;

        [StringLength(100)] public string Content { get; set; } = null!;
        public int Score { get; set; }
    }
}
