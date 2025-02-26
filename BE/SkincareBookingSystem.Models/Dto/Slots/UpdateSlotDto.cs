﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.Slots
{
    public class UpdateSlotDto
    {
        public Guid SlotId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? UpdatedBy { get; set; } = null;
        public string? Status { get; set; }
    }
}
