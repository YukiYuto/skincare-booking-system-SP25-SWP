using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class SkinServiceType
    {
        public Guid SkinProfileID { get; set; }
        [ForeignKey("SkinProfileID")]
        public virtual SkinProfile SkinProfile { get; set; } = null!;

        public Guid ServiceTypeID { get; set; }
        [ForeignKey("ServiceTypeID")]
        public virtual ServiceType ServiceType { get; set; } = null!;
    }
}
