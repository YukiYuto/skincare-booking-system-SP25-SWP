using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class OrderDetail
    {
        [Key]
        public Guid OrderDetailId { get; set; }

        public Guid OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; } = null!;

        public Guid? ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Services? Services { get; set; }

        public Guid? ServiceComboId { get; set; }
        [ForeignKey("ServiceComboId")]
        public virtual ServiceCombo? ServiceCombo { get; set; }

        public double Price { get; set; }
        [StringLength(100)] public string Description { get; set; } = null!;
    }
}
