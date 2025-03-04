using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TherapistServiceTypes
{
    public class ServiceTypeDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; } = null!;
    }
}
