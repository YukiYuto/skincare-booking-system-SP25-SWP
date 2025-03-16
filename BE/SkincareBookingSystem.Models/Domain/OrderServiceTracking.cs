using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public class OrderServiceTracking : BaseEntity<string, string, string>
{
    [Key] public Guid TrackingId { get; set; }

    [Required] public Guid OrderDetailId { get; set; }

    [ForeignKey("OrderDetailId")] public virtual OrderDetail OrderDetail { get; set; } = null!;
}