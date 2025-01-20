using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class DurationItem
    {
        public Guid ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Services Services { get; set; } = null!;

        public Guid DurationID { get; set; }
        [ForeignKey("DurationID")]
        public virtual Duration Duration { get; set; } = null!;
    }
}
