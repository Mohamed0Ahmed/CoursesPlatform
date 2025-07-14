using Courses.Domain.Identity;

namespace Courses.Application.Abstraction.Email;

public interface IEmailService
{
    Task<bool> SendWelcomeEmailAsync(string email, string fullName);
    Task<bool> SendVerificationCodeAsync(string email, string code);
    Task<bool> SendPasswordResetEmailAsync(string email, string resetLink);
    Task<bool> SendCourseEnrollmentEmailAsync(string email, string courseName);
} 