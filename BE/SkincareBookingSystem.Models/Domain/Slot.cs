﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class Slot : BaseEntity<string, string, string>
    {
        [Key]
        public Guid SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        
        public virtual ICollection<TherapistSchedule> TherapistSchedules { get; set; } = new List<TherapistSchedule>();
    }
}
