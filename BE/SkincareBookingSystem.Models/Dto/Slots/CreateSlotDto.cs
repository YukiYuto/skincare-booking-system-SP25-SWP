using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Slots
{
    public class CreateSlotDto
    {
        [Required]public DateTime StartTime { get; set; }
        [Required]public DateTime EndTime { get; set; }
        public string? CreatedBy { get; set; } = null;
    }
}
