namespace Courses.Application.Features.Authentication.Commands.Register.Instructors;

public record InstructorRegisterCommand(InstructorRegisterRequestDto Dto) : IRequest<RegisterResponseDto>;
//**********

public class InstructorRegisterCommandHandler : IRequestHandler<InstructorRegisterCommand, RegisterResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ILogger<InstructorRegisterCommandHandler> _logger;

    public InstructorRegisterCommandHandler(
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ITwoFactorService twoFactorService,
        ILogger<InstructorRegisterCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _emailService = emailService;
        _twoFactorService = twoFactorService;
        _logger = logger;
    }

    public async Task<RegisterResponseDto> Handle(InstructorRegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Registration attempt for email: {Email}, type: {UserType}", request.Dto.Email, UserType.Instructor);

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
            UserType = UserType.Instructor,
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

        // Add user to Instructor role
        var roleResult = await _userManager.AddToRoleAsync(user, "Instructor");
        if (!roleResult.Succeeded)
        {
            var errors = roleResult.Errors.Select(e => e.Description).ToList();
            _logger.LogWarning("Failed to add user {Email} to Instructor role: {Errors}", request.Dto.Email, string.Join(", ", errors));
            throw new InvalidOperationException($"Failed to assign role: {string.Join(", ", errors)}");
        }

        // Insert instructor record in the Instructor table after user creation and role assignment

        var instructor = new Instructor
        {
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = "Registration",
        };

        var instructorRepository = _unitOfWork.GetRepository<Instructor, Guid>();
        await instructorRepository.AddAsync(instructor);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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