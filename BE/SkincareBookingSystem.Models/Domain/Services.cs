using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class Services : BaseEntity<string, string, string>
    {
        [Key] public Guid ServiceId { get; set; }
        [StringLength(50)] public string ServiceName { get; set; } = null!;
        [StringLength(200)] public string Description { get; set; } = null!;
        public double Price { get; set; }
        [StringLength(200)] public string? ImageUrl { get; set; }

        public virtual ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual ICollection<TypeItem> TypeItems { get; set; } = new List<TypeItem>();
        public virtual ICollection<DurationItem> DurationItems { get; set; } = new List<DurationItem>();
    }
}