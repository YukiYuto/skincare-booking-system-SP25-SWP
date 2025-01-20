using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; }

        public string UserID { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid SkinProfileID { get; set; }
        [ForeignKey("SkinProfileID")] public virtual SkinProfile SkinProfile { get; set; } = null!;

    }
}
