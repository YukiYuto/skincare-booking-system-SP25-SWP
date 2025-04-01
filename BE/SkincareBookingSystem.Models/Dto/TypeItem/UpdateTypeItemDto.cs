using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.TypeItem
{
    public class UpdateTypeItemDto
    {
        public Guid ServiceId { get; set; }
        public List<Guid> ServiceTypeIdList { get; set; }
    }
}
