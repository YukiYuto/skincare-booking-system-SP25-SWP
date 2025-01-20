using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class SkinTherapist
    {
        [Key]
        public Guid SkinTherapistID { get; set; }

        public string UserID { get; set; } = null!;
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [StringLength(20)] public string Experience { get; set; } = null!;

        public Guid TherapistScheduleID {  get; set; }
        [ForeignKey("TherapistScheduleID")]
        public virtual TherapistSchedule TherapistSchedule { get; set; } = null!;
    }
}
