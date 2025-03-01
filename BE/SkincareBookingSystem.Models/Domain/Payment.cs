using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkincareBookingSystem.Models.Domain;

public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Cancelled = 2
}

public class Payment
{
    [Key]
    public Guid PaymentTransactionId { get; set; }
    public long? OrderNumber { get; set; }
    [ForeignKey("OrderNumber")] public virtual Order? Orders { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public string? CancelUrl { get; set; }
    public string? ReturnUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public PaymentStatus Status { get; set; }
}