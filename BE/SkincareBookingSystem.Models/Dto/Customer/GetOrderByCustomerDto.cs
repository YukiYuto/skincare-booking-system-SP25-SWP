using SkincareBookingSystem.Models.Domain;

namespace SkincareBookingSystem.Models.Dto.Customer;

public class GetOrderByCustomerDto
{
    public Guid OrderId { get; set; }
    public DateTime CreatedTime { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public TransactionInfo? Transaction { get; set; }
}

public class TransactionInfo
{
    public decimal Amount { get; set; }
    public DateTime TransactionTime { get; set; }
    public Guid TransactionId { get; set; }
} 