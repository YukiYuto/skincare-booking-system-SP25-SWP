using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Utilities.Templates.Email;

namespace SkincareBookingSystem.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly bool _useSsl;

        public EmailService(IConfiguration configuration)
        {
            _fromEmail = configuration[StaticEmailSettings.FromEmail]!;
            _fromPassword = configuration[StaticEmailSettings.FromPassword]!;
            _smtpHost = configuration[StaticEmailSettings.SmtpHost]!;
            _smtpPort = int.Parse(configuration[StaticEmailSettings.SmtpPort]!);
            _useSsl = bool.Parse(configuration[StaticEmailSettings.UseSsl]!);
        }


        public async Task<bool> SendGooglePasswordEmailTemplate(
            string toEmail,
            string fullName,
            string password,
            string accountPageUrl
        )
        {
            var template = new GooglePasswordEmailTemplate();
            var placeholders = new Dictionary<string, string>
            {
                { "{{FullName}}", fullName },
                { "{{Password}}", password },
                { "{{AccountPageUrl}}", accountPageUrl }
            };
            return await SendEmailFromTemplateAsync(toEmail, template, placeholders);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetPasswordLink)
        {
            var template = new PasswordResetEmailTemplate();
            var placeholders = new Dictionary<string, string>
            {
                { "{{ResetPasswordLink}}", resetPasswordLink }
            };
            return await SendEmailFromTemplateAsync(toEmail, template, placeholders);
        }

        public async Task<bool> SendBookingSuccessEmailAsync(string toEmail,
            string userName,
            string bookingDateTime,
            List<string> bookingServices,
            string viewOrderLink)
        {
            string serviceListFormatted =
                "<ul>" +
                string.Join("", bookingServices.Select(service => $"<li>{service}</li>")) +
                "</ul>";
            var template = new BookingSuccessEmailTemplate();
            var placeholders = new Dictionary<string, string>
            {
                { "{{UserName}}", userName },
                { "{{ServiceList}}", serviceListFormatted },
                { "{{AppointmentDateTime}}", bookingDateTime },
                { "{{ViewOrderLink}}", viewOrderLink }
            };

            return await SendEmailFromTemplateAsync(toEmail, template, placeholders);
        }

        public async Task<bool> SendVerificationEmailAsync(string toEmail,
            string emailConfirmationLink,
            string fullName)
        {
            var template = new VerificationEmailTemplate();
            var placeholders = new Dictionary<string, string>
            {
                { "{{EmailConfirmationLink}}", emailConfirmationLink },
                { "{{UserName}}", fullName }
            };
            return await SendEmailFromTemplateAsync(toEmail, template, placeholders);
        }


        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(StaticEmailSettings.SenderName, _fromEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = body };
                message.Body = builder.ToMessageBody();

                // Connect to the SMTP server and send the email
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpHost, _smtpPort, _useSsl);
                    await client.AuthenticateAsync(_fromEmail, _fromPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> SendEmailFromTemplateAsync(string toEmail,
            GenericEmailTemplate template,
            Dictionary<string, string> placeholders)
        {
            string body = template.Render(placeholders);
            return await SendEmailAsync(toEmail, template.Subject, body);
        }
    }
}