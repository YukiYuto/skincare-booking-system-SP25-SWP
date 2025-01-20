using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class CustomerSkinTest
    {
        [Key]
        public Guid CustomerSkinTestID { get; set; }
        public Guid CustomerID { get; set; }
        public Guid SkinTestID { get; set; }
        public int Score { get; set; }
        public DateTime TakeAt { get; set; }
    }
}
