using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class SkinServiceType
    {
        public Guid SkinProfileId { get; set; }
        [ForeignKey("SkinProfileId")]
        public virtual SkinProfile SkinProfile { get; set; } = null!;

        public Guid ServiceTypeId { get; set; }
        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; } = null!;
    }
}
