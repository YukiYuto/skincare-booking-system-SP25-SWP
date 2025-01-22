using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Appointments : BaseEntity<string, string, string>
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; } = null!;
        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")] public virtual Order Order { get; set; } = null!;
        public DateTime? AppointmentDate { get; set; }
        [StringLength(30)]public string? AppointmentTime { get; set; }
    }
}
