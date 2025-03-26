using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.SkinProfile
{
    public class UpdateSkinProfileDto
    {
        public Guid? SkinProfileId { get; set; }
        public string? SkinName { get; set; }
        public Guid? ParentSkin { get; set; }
        public string? Description { get; set; } 
        public int? ScoreMin { get; set; }
        public int? ScoreMax { get; set; }
    }
}
