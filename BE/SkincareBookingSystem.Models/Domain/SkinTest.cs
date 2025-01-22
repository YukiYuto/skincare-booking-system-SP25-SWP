using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkincareBookingSystem.Models.Domain
{
    public class SkinTest: BaseEntity<string, string, string>
    {
        [Key]
        public Guid SkinTestId { get; set; }
        [StringLength(30)] public string SkinTestName { get; set; } = null!;
        [StringLength(30)] public string Description { get; set; } = null!;
        
        public Guid CustomerSkinTestId { get; set; }
        [ForeignKey("CustomerSkinTestId")] 
        public virtual CustomerSkinTest CustomerSkinTest { get; set; } = null!;
    }
}
