using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Appointments
    {
        [Key]
        public Guid AppointmentId { get; set; }
        public Guid CustomerID { get; set; }
        [ForeignKey("CustomerID")] public virtual Customer Customer { get; set; } = null!;
        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")] public virtual Order Order { get; set; } = null!;
        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentTime { get; set; }
        public string? Status { get; set; }
    }
}
