using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class ComboItem
    {
        public Guid ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Services Services { get; set; } = null!;

        public Guid ServiceComboID { get; set; }
        [ForeignKey("ServiceComboID")]
        public virtual ServiceCombo ServiceCombo { get; set; } = null!;
    }
}
