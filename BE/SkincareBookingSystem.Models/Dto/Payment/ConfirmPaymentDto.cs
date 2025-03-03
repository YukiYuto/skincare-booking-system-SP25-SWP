namespace SkincareBookingSystem.Models.Dto.Payment;

public class ConfirmPaymentDto
{
    public long orderNumber { get; set; }
    public Guid paymentTransactionId { get; set; }
}