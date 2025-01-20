using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class ServiceType: BaseEntity<string, string, string>
    {
        [Key]
        public Guid ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; } = null!;
    }
}
