using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TherapistServiceTypes
{
    public class TherapistDto
    {
        public Guid SkinTherapistId { get; set; }
        public List<ServiceTypeDto>? ServiceTypes { get; set; }
    }
}
