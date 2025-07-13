namespace Courses.Application.Abstraction
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendVerificationCodeAsync(string to, string code);
        Task<bool> SendPasswordResetAsync(string to, string resetLink);
        Task<bool> SendWelcomeEmailAsync(string to, string userName);
    }
}