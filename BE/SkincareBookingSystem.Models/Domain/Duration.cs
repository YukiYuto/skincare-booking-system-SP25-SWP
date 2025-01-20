using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class Duration : BaseEntity<string, string, string>
    {
        [Key]
        public Guid DurationID { get; set; }
        [StringLength(20)] public string DurationTime { get; set; } = null!;
    }
}
