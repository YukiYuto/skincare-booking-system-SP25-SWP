﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Services.IServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendVerificationEmailAsync(string toEmail, string emailConfirmationLink);
        Task<bool> SendPasswordResetEmailAsync(string toEmail, string resetPasswordLink);
    }
}
