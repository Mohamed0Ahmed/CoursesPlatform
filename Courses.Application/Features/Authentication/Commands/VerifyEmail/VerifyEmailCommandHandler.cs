
namespace Courses.Application.Features.Authentication.Commands.VerifyEmail;

public record VerifyEmailCommand(VerifyCodeRequestDto Dto, UserType UserType) : IRequest<VerifyEmailResponseDto>;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, VerifyEmailResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITwoFactorService _twoFactorService;
    private readonly IJwtService _jwtService;
    private readonly ILogger<VerifyEmailCommandHandler> _logger;

    public VerifyEmailCommandHandler(
        UserManager<ApplicationUser> userManager,
        ITwoFactorService twoFactorService,
        IJwtService jwtService,
        ILogger<VerifyEmailCommandHandler> logger)
    {
        _userManager = userManager;
        _twoFactorService = twoFactorService;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<VerifyEmailResponseDto> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email verification attempt for email: {Email}", request.Dto.Email);

        var user = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Email verification failed: User not found for email {Email}", request.Dto.Email);
            throw new InvalidOperationException("User not found");
        }
        
        if (user.UserType != request.UserType)
        {
            _logger.LogWarning("Verification failed: User {Email} is not of type {UserType}", request.Dto.Email, request.UserType);
            throw new UnauthorizedAccessException($"This verification is for {request.UserType}s only");
        }

        var isValid = await _twoFactorService.ValidateVerificationCodeAsync(user, request.Dto.Code);
        if (!isValid)
        {
            _logger.LogWarning("Email verification failed: Invalid code for email {Email}", request.Dto.Email);
            throw new InvalidOperationException("Invalid or expired verification code");
        }

        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        var token = _jwtService.GenerateToken(user);
        var userInfo = user.Adapt<UserInfoDto>();

        return new VerifyEmailResponseDto
        {
            Message = "Email verified successfully!",
            Token = token,
            User = userInfo
        };
    }
} 