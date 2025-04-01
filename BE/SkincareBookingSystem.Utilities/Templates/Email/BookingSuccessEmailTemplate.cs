using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Utilities.Templates.Email;

public class BookingSuccessEmailTemplate : GenericEmailTemplate
{
    public override string TemplateName { get; set; } = StaticEmailTemplates.BookingSuccess;
    public override string Subject { get; set; } = "Thank you for your booked!";
    public override string Category { get; set; } = "BookingSuccess";
    public override string PreHeaderText { get; set; } = "Your appointment has been successfully booked!";

    public override string BodyContent { get; set; } =
        "<p>Dear {{UserName}},</p>" +
        "<p>We’re excited to let you know that your booking has been successfully confirmed. Here are the details of your appointment:</p>" +
        "<p><strong>Services:</strong><br>{{ServiceList}}</p>" +
        "<p><strong>Date & Time:</strong> {{AppointmentDateTime}}</p>" +
        "<p>If you need to reschedule or have any questions, please feel free to contact us. We look forward to serving you!</p>";

    public override string CallToAction { get; set; } = "{{ViewOrderLink}}";
    public override string CallToActionText { get; set; } = "Manage Your Booking";

    public override string FooterContent { get; set; } =
        $"If you have any questions, feel free to contact us at {StaticEmailSettings.SenderEmail}.";

    public override string RecipientType { get; set; } = StaticUserRoles.Customer;
    
    public BookingSuccessEmailTemplate() { }
}