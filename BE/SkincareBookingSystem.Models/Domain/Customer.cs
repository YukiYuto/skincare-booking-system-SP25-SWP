using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        public string UserId { get; set; } = null!;
        [ForeignKey("UserId")] public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid? SkinProfileId { get; set; }
        [ForeignKey("SkinProfileId")] public virtual SkinProfile SkinProfile { get; set; } = null!;
        
        public virtual ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
        public virtual ICollection<CustomerSkinTest> CustomerSkinTests { get; set; } = new List<CustomerSkinTest>();
    }
}
