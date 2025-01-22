using System.ComponentModel.DataAnnotations;


namespace SkincareBookingSystem.Models.Domain
{
    public class ServiceCombo : BaseEntity<string, string, string>
    {
        [Key]
        public Guid ServiceComboId { get; set; }
        [StringLength(50)] public string ComboName { get; set; } = null!;
        [StringLength(100)] public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int NumberOfService { get; set; }
        
        public virtual ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
