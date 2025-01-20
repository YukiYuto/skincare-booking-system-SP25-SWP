using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Services : BaseEntity<string, string, string>
    {
        [Key]
        public Guid ServiceID { get; set; }
        [StringLength(50)] public string ServiceName { get; set; } = null!;
        [StringLength(50)] public string Description { get; set; } = null!;
        public double Price { get; set; }

        public Guid ServiceTypeID { get; set; }
        [ForeignKey("ServiceTypeID")]
        public virtual ServiceType ServiceType { get; set; }
    }
}
