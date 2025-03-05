namespace SkincareBookingSystem.Models.Dto.Payment;

public class GetAllPaymentDto
{
    public Guid PaymentTransactionId { get; set; }
    public long? OrderNumber { get; set; }
    public int Amount { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}