namespace SkincareBookingSystem.Models.Dto.CustomerSkinTest;

public class CreateCustomerSkinTestDto
{
    public Guid CustomerId { get; set; }
    public Guid SkinTestId { get; set; }
    public List<TestAnswerDto> Answers { get; set; } = new List<TestAnswerDto>();
}