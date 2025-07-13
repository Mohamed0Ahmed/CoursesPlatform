using Courses.Domain.Identity;

namespace Courses.Application.Abstraction
{
    public interface ITwoFactorService
    {
        Task<string> GenerateVerificationCodeAsync(ApplicationUser user);
        Task<bool> ValidateVerificationCodeAsync(ApplicationUser user, string code);
        Task<bool> SendVerificationCodeAsync(ApplicationUser user);
        Task<bool> IsVerificationCodeExpiredAsync(ApplicationUser user);
    }
}