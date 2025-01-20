using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class SkinProfile : BaseEntity<string, string, string>
    {
        [Key]
        public Guid SkinProfileID { get; set; }
        [StringLength(30)] public string SkinName { get; set; } = null!;
        public Guid? ParentSkin { get; set; }

        [StringLength(30)] public string Description { get; set; } = null!;
        public int Score { get; set; }

    }
}
