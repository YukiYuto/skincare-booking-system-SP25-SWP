using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TherapistServiceTypes
{
    public class TherapistServiceTypesDto
    {
        public Guid TherapistId { get; set; }
        public List<Guid> ServiceTypeIdList { get; set; } = [];
    }
}
