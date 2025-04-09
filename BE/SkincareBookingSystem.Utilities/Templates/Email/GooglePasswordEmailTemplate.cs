using SkincareBookingSystem.Utilities.Constants;

namespace SkincareBookingSystem.Utilities.Templates.Email;

/// <summary>
///     GooglePasswordEmailTemplate: A class that represents an email template for sending
///     auto-generated passwords to users who register via Google login
/// </summary>
public class GooglePasswordEmailTemplate : GenericEmailTemplate
{
    public GooglePasswordEmailTemplate()
    {
        TemplateName = "GooglePasswordEmail";
        Subject = "Your New Account Password for Skincare Booking System";
        PreHeaderText = "Your account has been created successfully with Google Sign-In";
        Category = "Authentication";
        RecipientType = "NewUser";

        BodyContent = @"
            <p>Hello {{FullName}},</p>
            
            <p>Thank you for signing up with your Google account. Your Skincare Booking System account has been created successfully.</p>
            
            <p>To ensure you can also log in using our regular login system, we've generated a secure password for your account:</p>
            
            <div style='background-color: #f5f5f5; padding: 15px; border-radius: 5px; margin: 20px 0; text-align: center;'>
                <p style='font-family: monospace; font-size: 18px; font-weight: bold; letter-spacing: 1px; margin: 0;'>{{Password}}</p>
            </div>
            
            <p>For security purposes, we recommend changing this password to something you can easily remember after logging in.</p>
            
            <p>You can continue to use your Google account to sign in, or use your email address and this password.</p>";

        CallToActionText = "Go to My Account";
        CallToAction = "{{AccountPageUrl}}";
        
        FooterContent = @"
            <p>If you did not create an account with us, please disregard this email.</p>
            <p>For security reasons, please do not reply to this email with your password or other sensitive information.</p>
            <p>If you have any questions, please contact our support team at support@skincarebooking.com</p>";
    }
}