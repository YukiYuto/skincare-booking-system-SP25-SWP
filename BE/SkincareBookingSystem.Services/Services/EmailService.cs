﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Utilities.Constants;
using SkincareBookingSystem.Utilities.Templates.Email;

namespace SkincareBookingSystem.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly bool _useSsl;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _fromEmail = _configuration[StaticEmailSettings.FromEmail];
            _fromPassword = _configuration[StaticEmailSettings.FromPassword];
            _smtpHost = _configuration[StaticEmailSettings.SmtpHost];
            _smtpPort = int.Parse(_configuration[StaticEmailSettings.SmtpPort]);
            _useSsl = bool.Parse(_configuration[StaticEmailSettings.UseSsl]);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetPasswordLink)
        {
            return await SendEmailFromTemplateAsync(toEmail, new PasswordResetEmailTemplate(), new Dictionary<string, string> { { "ResetPasswordLink", resetPasswordLink } });
        }

        public async Task<bool> SendVerificationEmailAsync(string toEmail, string emailConfirmationLink)
        {
            return await SendEmailFromTemplateAsync(toEmail, new VerificationEmailTemplate(), new Dictionary<string, string> { { "EmailConfirmationLink", emailConfirmationLink } });
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

        private async Task<bool> SendEmailFromTemplateAsync(string toEmail, GenericEmailTemplate template, Dictionary<string, string> placeholders)
        {
            string body = template.Render(placeholders);
            return await SendEmailAsync(toEmail, template.Subject, body);
        }
    }
}
