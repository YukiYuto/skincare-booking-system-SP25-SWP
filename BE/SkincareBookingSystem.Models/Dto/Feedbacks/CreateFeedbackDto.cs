using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Feedbacks
{
    public class CreateFeedbackDto
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Rating { get; set; }
        public Guid AppointmentId { get; set; } 
    }
}
