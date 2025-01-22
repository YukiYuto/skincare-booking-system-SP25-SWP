using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class CustomerSkinTest
    {
        [Key]
        public Guid CustomerSkinTestId { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; } = null!;
        public int Score { get; set; }
        public DateTime TakeAt { get; set; }
        
        public virtual ICollection<SkinTest> SkinTests { get; set; } = new List<SkinTest>();
    }
}
