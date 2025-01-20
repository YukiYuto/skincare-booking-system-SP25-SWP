using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Feedbacks : BaseEntity<string, string, string>
    {
        [Key]
        public Guid FeedbackID { get; set; }
        [StringLength(30)] public string Title { get; set; } = null!;
        [StringLength(100)] public string Content { get; set; } = null!;
        [Range(1, 5)]public int Rating { get; set; }

        public Guid AppointmentID { get; set; }
        [ForeignKey("AppointmentID")]
        public virtual Appointments Appointments { get; set; } = null!;
    }
}
