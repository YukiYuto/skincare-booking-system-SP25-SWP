using System.ComponentModel.DataAnnotations;


namespace SkincareBookingSystem.Models.Domain
{
    public class ServiceType: BaseEntity<string, string, string>
    {
        [Key]
        public Guid ServiceTypeId { get; set; }
        [StringLength(30)]public string ServiceTypeName { get; set; } = null!;
        [StringLength(100)]public string Description { get; set; } = null!;
        
        public virtual ICollection<SkinServiceType> SkinServiceTypes { get; set; } = new List<SkinServiceType>();
        public virtual ICollection<TypeItem> TypeItems { get; set; } = new List<TypeItem>();
        public virtual ICollection<TherapistServiceType> TherapistServiceTypes { get; set; } = new List<TherapistServiceType>();
    }
}
