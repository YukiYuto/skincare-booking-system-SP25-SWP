namespace SkincareBookingSystem.Models.Dto.Payment;

public class CreatePaymentLinkDto
{
    public long OrderNumber { get; set; }
    //public Guid CustomerId { get; set; }
    public string CancelUrl { get; set; } = null!;
    public string ReturnUrl { get; set; } = null!;
}