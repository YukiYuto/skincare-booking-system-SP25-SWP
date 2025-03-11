using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TestAnswer
{
    public class UpdateTestAnswerDto
    {
        public Guid TestAnswerId { get; set; }
        public Guid TestQuestionId { get; set; }
        public string Content { get; set; } = null!;
        public int Score { get; set; }
    }
}
