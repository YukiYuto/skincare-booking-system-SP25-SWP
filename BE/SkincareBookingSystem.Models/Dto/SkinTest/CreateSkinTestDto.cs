using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Dto.SkinTest
{
    public class CreateSkinTestDto
    {
        public string SkinTestName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid? CustomerSkinTestId { get; set; }
    }
}
