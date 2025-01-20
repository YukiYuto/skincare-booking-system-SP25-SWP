using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Order : BaseEntity<string, string, string>
    {
        [Key]
        public Guid OrderID { get; set; }

        public Guid CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; } = null!;

        public double TotalPrice { get; set; }
    }
}
