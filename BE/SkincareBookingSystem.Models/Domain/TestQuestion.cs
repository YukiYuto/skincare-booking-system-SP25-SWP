using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Models.Domain
{
    public class TestQuestion
    {
        [Key]
        public Guid TestQuestionID { get; set; }

        public Guid SkinTestID { get; set; }
        [ForeignKey("SkinTestID")]
        public virtual SkinTest SkinTest { get; set; } = null!;

        [StringLength(30)] public string Content { get; set; } = null!;
        [StringLength(30)] public string Type { get; set; } = null!;
    }
}
