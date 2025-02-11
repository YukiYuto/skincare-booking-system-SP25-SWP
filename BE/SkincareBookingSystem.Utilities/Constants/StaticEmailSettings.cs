using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkincareBookingSystem.Utilities.Constants
{
    public static class StaticEmailSettings
    {
        public const string EmailSettingsSection = "EmailSettings";

        public const string FromEmail = "EmailSettings:FromEmail";
        public const string FromPassword = "EmailSettings:FromPassword";
        public const string SmtpHost = "EmailSettings:SmtpHost";
        public const string SmtpPort = "EmailSettings:SmtpPort";
        public const string UseSsl = "EmailSettings:UseSsl";

        public const string SenderName = "LumiConnect";
        public const string SenderEmail = "lumiconnect.business@gmail.com";
    }
}
