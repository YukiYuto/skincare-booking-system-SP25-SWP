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
        public Guid ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Services Services { get; set; } = null!;

        public Guid ServiceTypeID { get; set; }
        [ForeignKey("ServiceTypeID")]
        public virtual ServiceType ServiceType { get; set; } = null!;
    }
}
