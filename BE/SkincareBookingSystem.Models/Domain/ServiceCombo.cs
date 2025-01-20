using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class ServiceCombo : BaseEntity<string, string, string>
    {
        [Key]
        public Guid ServiceComboID { get; set; }
        [StringLength(50)] public string ComboName { get; set; } = null!;
        [StringLength(100)] public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int NumberOfService { get; set; }
    }
}
