using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TestQuestion
{
    public class CreateTestQuestionDto
    {
        public Guid SkinTestId { get; set; }
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
