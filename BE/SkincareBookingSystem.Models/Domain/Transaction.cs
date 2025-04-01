using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Models.Domain;

public class Transaction
{
    [Key] public Guid TransactionId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? OrderId { get; set; }
    public Guid PaymentId { get; set; }
    public double Amount { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public string TransactionMethod { get; set; } = null!;

    [ForeignKey("CustomerId")] public virtual Customer Customer { get; set; }
    [ForeignKey("OrderId")] public virtual Order Orders { get; set; }
    [ForeignKey("PaymentId")] public virtual Payment Payment { get; set; }
}