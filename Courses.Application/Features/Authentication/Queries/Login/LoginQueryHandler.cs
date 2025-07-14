namespace Courses.Application.Features.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly ILogger<LoginQueryHandler> _logger;

    public LoginQueryHandler(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        ILogger<LoginQueryHandler> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<LoginResponseDto> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for email: {Email}, type: {UserType}", request.Dto.Email, request.UserType);

        var user = await _userManager.FindByEmailAsync(request.Dto.Email);
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found for email {Email}", request.Dto.Email);
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (user.UserType != request.UserType)
        {
            _logger.LogWarning("Login failed: User {Email} is not of type {UserType}", request.Dto.Email, request.UserType);
            throw new UnauthorizedAccessException($"This login is for {request.UserType}s only");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Dto.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Login failed: Invalid password for email {Email}", request.Dto.Email);
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: Deactivated account for email {Email}", request.Dto.Email);
            throw new UnauthorizedAccessException("Account is deactivated");
        }

        var token = _jwtService.GenerateToken(user);
        var userInfo = user.Adapt<UserInfoDto>();
        
        return new LoginResponseDto
        {
            Token = token,
            User = userInfo
        };
    }
} 