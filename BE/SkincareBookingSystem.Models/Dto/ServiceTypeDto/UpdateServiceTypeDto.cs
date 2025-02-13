using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.ServiceTypeDto
{
    public class UpdateServiceTypeDto
    {
        public Guid ServiceTypeId { get; set; }
        [StringLength(30)]
        public string ServiceTypeName { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
