using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.SkinProfile
{
    public class CreateSkinProfileDto
    {
        public string SkinName { get; set; } = null!;
        public Guid? ParentSkin { get; set; }
        public string Description { get; set; } = null!;
        public int Score { get; set; }
    }
}
