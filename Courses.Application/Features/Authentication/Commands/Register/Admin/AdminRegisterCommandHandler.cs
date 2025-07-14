namespace Courses.Application.Features.Authentication.Commands.Register.Admin;

public class AdminRegisterCommandHandler : IRequestHandler<AdminRegisterCommand, RegisterResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ILogger<AdminRegisterCommandHandler> _logger;

    public AdminRegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ITwoFactorService twoFactorService,
        ILogger<AdminRegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _emailService = emailService;
        _twoFactorService = twoFactorService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(AdminRegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registration attempt for email: {Email}, type: {UserType}", request.Dto.Email, UserType.Admin);

        var existingUser = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Registration failed: Email already exists {Email}", request.Dto.Email);
            throw new InvalidOperationException("Email is already registered");
        }

        var user = new ApplicationUser
        {
            UserName = request.Dto.Email,
            Email = request.Dto.Email,
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName,
            UserType = UserType.Admin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, request.Dto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            _logger.LogWarning("Registration failed for email {Email}: {Errors}", request.Dto.Email, string.Join(", ", errors));
            throw new InvalidOperationException(string.Join(", ", errors));
        }

        await _emailService.SendWelcomeEmailAsync(user.Email!, user.FullName);
        await _twoFactorService.SendVerificationCodeAsync(user);
        
        var userInfo = user.Adapt<UserInfoDto>();

        return new RegisterResponseDto
        {
            Message = "Registration successful. Please check your email for verification code.",
            User = userInfo
        };
    }
} 