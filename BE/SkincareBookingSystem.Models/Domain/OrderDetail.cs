using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailID { get; set; }

        public Guid OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; } = null!;

        public Guid ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public virtual Services? Services { get; set; }

        public Guid ServiceComboID { get; set; }
        [ForeignKey("ComboID")]
        public virtual ServiceCombo? ServiceCombo { get; set; }

        public double Price { get; set; }
        [StringLength(100)] public string Description { get; set; } = null!;
    }
}
