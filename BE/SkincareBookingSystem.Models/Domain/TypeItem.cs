using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class TypeItem
    {
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Services Services { get; set; } = null!;

        public Guid ServiceTypeId { get; set; }
        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; } = null!;
    }
}
