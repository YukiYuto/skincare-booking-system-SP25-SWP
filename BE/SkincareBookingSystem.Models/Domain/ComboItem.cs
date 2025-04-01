using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class ComboItem
    {
        public Guid ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public virtual Services Services { get; set; } = null!;

        public Guid ServiceComboId { get; set; }
        [ForeignKey("ServiceComboId")]
        public virtual ServiceCombo ServiceCombo { get; set; } = null!;
        
        public int Priority { get; set; }
    }
}
