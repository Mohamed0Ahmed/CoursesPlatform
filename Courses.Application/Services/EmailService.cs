using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Courses.Application.Abstraction;

namespace Courses.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"] ?? "";
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"] ?? "";
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? "";
            _fromName = _configuration["EmailSettings:FromName"] ?? "Courses Platform";
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_smtpServer, _smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception in production
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendVerificationCodeAsync(string to, string code)
        {
            var subject = "Email Verification Code - Courses Platform";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #333;'>Email Verification</h2>
                    <p>Your verification code is:</p>
                    <div style='background-color: #f4f4f4; padding: 20px; text-align: center; margin: 20px 0;'>
                        <h1 style='color: #007bff; font-size: 32px; margin: 0;'>{code}</h1>
                    </div>
                    <p>This code will expire in 10 minutes.</p>
                    <p>If you didn't request this code, please ignore this email.</p>
                    <hr>
                    <p style='color: #666; font-size: 12px;'>This is an automated message from Courses Platform.</p>
                </div>";

            return await SendEmailAsync(to, subject, body);
        }

        public async Task<bool> SendPasswordResetAsync(string to, string resetLink)
        {
            var subject = "Password Reset - Courses Platform";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #333;'>Password Reset Request</h2>
                    <p>You have requested to reset your password.</p>
                    <p>Click the button below to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' style='background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; display: inline-block;'>Reset Password</a>
                    </div>
                    <p>If you didn't request this reset, please ignore this email.</p>
                    <p>This link will expire in 1 hour.</p>
                    <hr>
                    <p style='color: #666; font-size: 12px;'>This is an automated message from Courses Platform.</p>
                </div>";

            return await SendEmailAsync(to, subject, body);
        }

        public async Task<bool> SendWelcomeEmailAsync(string to, string userName)
        {
            var subject = "Welcome to Courses Platform";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #333;'>Welcome to Courses Platform!</h2>
                    <p>Hello {userName},</p>
                    <p>Thank you for joining Courses Platform. We're excited to have you on board!</p>
                    <p>You can now:</p>
                    <ul>
                        <li>Browse our courses</li>
                        <li>Enroll in your favorite courses</li>
                        <li>Track your learning progress</li>
                        <li>Connect with instructors</li>
                    </ul>
                    <p>If you have any questions, feel free to contact our support team.</p>
                    <hr>
                    <p style='color: #666; font-size: 12px;'>This is an automated message from Courses Platform.</p>
                </div>";

            return await SendEmailAsync(to, subject, body);
        }
    }
}