namespace Courses.Application.Features.Authentication.Commands.Register.Students
{

    public record StudentRegisterCommand(StudentRegisterRequestDto Dto) : IRequest<RegisterResponseDto>;
    //**********

    public class StudentRegisterCommandHandler : IRequestHandler<StudentRegisterCommand, RegisterResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ITwoFactorService _twoFactorService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StudentRegisterCommandHandler> _logger;

        public StudentRegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            ITwoFactorService twoFactorService,
            IUnitOfWork unitOfWork,
            ILogger<StudentRegisterCommandHandler> logger)
        {
            _userManager = userManager;
            _emailService = emailService;
            _twoFactorService = twoFactorService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RegisterResponseDto> Handle(StudentRegisterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registration attempt for email: {Email}, type: {UserType}", request.Dto.Email, UserType.Student);

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
                UserType = UserType.Student,
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

            // Add user to Student role
            var roleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Failed to add user {Email} to Student role: {Errors}", request.Dto.Email, string.Join(", ", errors));
                throw new InvalidOperationException($"Failed to assign role: {string.Join(", ", errors)}");
            }

            // Create Student record
            var student = new Student
            {
                UserId = user.Id,
                EnrollmentDate = DateTime.UtcNow,
                CreatedBy = "Registration",
            };

            var studentRepository = _unitOfWork.GetRepository<Student, Guid>();
            await studentRepository.AddAsync(student);
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
}