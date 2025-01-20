using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class TestAnswer
    {
        [Key]
        public Guid TestAnswerID { get; set; }

        public Guid TestQuestionID { get; set; }
        [ForeignKey("TestQuestionID")]
        public virtual TestQuestion TestQuestion { get; set; } = null!;

        [StringLength(100)] public string Content { get; set; } = null!;
        public int Score { get; set; }
    }
}
