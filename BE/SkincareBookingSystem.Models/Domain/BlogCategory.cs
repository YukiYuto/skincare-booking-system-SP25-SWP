using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class BlogCategory : BaseEntity<string, string, string>
    {
        [Key]
        public Guid BlogCategoryID { get; set; }
        [StringLength(30)] public string Name { get; set; } = null!;
        [StringLength(100)] public string Description { get; set; } = null!;
    }
}
