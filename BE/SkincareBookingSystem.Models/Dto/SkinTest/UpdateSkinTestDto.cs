using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.SkinTest
{
    public class UpdateSkinTestDto
    {
        public Guid SkinTestId { get; set; }
        public string? SkinTestName { get; set; } 
        public string? Description { get; set; } 
        public Guid CustomerSkinTestId { get; set; }
    }
}
