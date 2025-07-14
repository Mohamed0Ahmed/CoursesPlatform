using Courses.Domain.Identity;

namespace Courses.Application.Abstraction.TwoFactor;

public interface ITwoFactorService
{
    Task<bool> SendVerificationCodeAsync(ApplicationUser user);
    Task<bool> ValidateVerificationCodeAsync(ApplicationUser user, string code);
    Task<string> GenerateVerificationCodeAsync();
    Task<bool> IsCodeExpiredAsync(ApplicationUser user);
} 