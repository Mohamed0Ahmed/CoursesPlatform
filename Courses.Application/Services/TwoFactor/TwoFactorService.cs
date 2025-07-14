using Courses.Application.Abstraction.TwoFactor;
using Courses.Application.Abstraction.Email;
using Courses.Domain.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Courses.Application.Services.TwoFactor;

public class TwoFactorService : ITwoFactorService
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<TwoFactorService> _logger;

    public TwoFactorService(IEmailService emailService, ILogger<TwoFactorService> logger, UserManager<ApplicationUser> userManager)
    {
        _emailService = emailService;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<bool> SendVerificationCodeAsync(ApplicationUser user)
    {
        try
        {
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            
            var emailSent = await _emailService.SendVerificationCodeAsync(user.Email!, code);
            
            if (emailSent)
            {
                _logger.LogInformation("Verification code sent successfully to {Email}", user.Email);
                return true;
            }
            
            _logger.LogError("Failed to send verification code to {Email}", user.Email);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending verification code to {Email}", user.Email);
            return false;
        }
    }

    public async Task<bool> ValidateVerificationCodeAsync(ApplicationUser user, string code)
    {
        try
        {
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", code);

            if (isValid)
            {
                _logger.LogInformation("Verification code validated successfully for {Email}", user.Email);
                return true;
            }

            _logger.LogWarning("Invalid verification code for {Email}", user.Email);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating verification code for {Email}", user.Email);
            return false;
        }
    }

    public Task<string> GenerateVerificationCodeAsync()
    {
        // This is now handled by Identity's GenerateTwoFactorTokenAsync
        throw new NotImplementedException();
    }

    public Task<bool> IsCodeExpiredAsync(ApplicationUser user)
    {
        // Identity tokens have a limited lifetime by default, so this check is implicitly handled by ValidateVerificationCodeAsync
        throw new NotImplementedException();
    }
} 