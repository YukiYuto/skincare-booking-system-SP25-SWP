using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class BaseEntity<CID, UID, SID>
    {
        public CID? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public UID? UpdatedBy { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public SID? Status { get; set; }
    }
}
