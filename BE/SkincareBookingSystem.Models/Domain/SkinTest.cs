using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class SkinTest: BaseEntity<string, string, string>
    {
        [Key]
        public Guid SkinTestID { get; set; }
        [StringLength(30)] public string SkinTestName { get; set; } = null!;
        [StringLength(30)] public string Description { get; set; } = null!;
    }
}
