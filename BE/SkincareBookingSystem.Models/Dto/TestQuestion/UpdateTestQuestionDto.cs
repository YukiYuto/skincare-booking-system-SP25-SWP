using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TestQuestion
{
    public class UpdateTestQuestionDto
    {
        public Guid TestQuestionId { get; set; }
        public string? Content { get; set; } 
        public string? Type { get; set; } 
        public Guid SkinTestId { get; set; }
    }
}
