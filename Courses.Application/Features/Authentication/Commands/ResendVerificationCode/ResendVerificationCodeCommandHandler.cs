namespace Courses.Application.Features.Authentication.Commands.ResendVerificationCode;

public record ResendVerificationCodeCommand(SendVerificationCodeRequestDto Dto, UserType UserType) : IRequest<bool>;

//***************
public class ResendVerificationCodeCommandHandler : IRequestHandler<ResendVerificationCodeCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ILogger<ResendVerificationCodeCommandHandler> _logger;

    public ResendVerificationCodeCommandHandler(
        UserManager<ApplicationUser> userManager,
        ITwoFactorService twoFactorService,
        ILogger<ResendVerificationCodeCommandHandler> logger)
    {
        _userManager = userManager;
        _twoFactorService = twoFactorService;
        _logger = logger;
    }

    public async Task<bool> Handle(ResendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Resend verification attempt for email: {Email}", request.Dto.Email);

        var user = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Resend verification failed: User not found for email {Email}", request.Dto.Email);
            throw new InvalidOperationException("User not found");
        }
        
        if (user.UserType != request.UserType)
        {
            _logger.LogWarning("Resend verification failed: User {Email} is not of type {UserType}", request.Dto.Email, request.UserType);
            throw new UnauthorizedAccessException($"This action is for {request.UserType}s only");
        }

        if (user.EmailConfirmed)
        {
            _logger.LogWarning("Resend verification failed: Email already verified for {Email}", request.Dto.Email);
            throw new InvalidOperationException("Email is already verified");
        }

        var emailSent = await _twoFactorService.SendVerificationCodeAsync(user);
        if (!emailSent)
        {
            _logger.LogError("Resend verification failed: Could not send code for email {Email}", request.Dto.Email);
            throw new InvalidOperationException("Failed to send verification code. Please try again.");
        }

        _logger.LogInformation("Verification code resent successfully for user: {UserId}", user.Id);
        return true;
    }
} 