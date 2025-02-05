using System.ComponentModel.DataAnnotations;


namespace SkincareBookingSystem.Models.Domain
{
    public class SkinProfile : BaseEntity<string, string, string>
    {
        [Key]
        public Guid SkinProfileId { get; set; }
        [StringLength(30)] public string SkinName { get; set; } = null!;
        public Guid? ParentSkin { get; set; }

        [StringLength(30)] public string Description { get; set; } = null!;
        public int Score { get; set; }
        
        public virtual ICollection<SkinServiceType> SkinServiceTypes { get; set; } = new List<SkinServiceType>();
    }
}
