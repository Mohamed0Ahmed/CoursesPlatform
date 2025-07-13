namespace Courses.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/instructor")]
    public class InstructorAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ITwoFactorService _twoFactorService;
        private readonly ILogger<InstructorAuthController> _logger;

        public InstructorAuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IEmailService emailService,
            ITwoFactorService twoFactorService,
            ILogger<InstructorAuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _twoFactorService = twoFactorService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            _logger.LogInformation("Instructor login attempt for email: {Email}", request.Email);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Instructor login failed: User not found for email {Email}", request.Email);
                return this.Unauthorized<LoginResponseDto>("Invalid email or password");
            }

            // Check if user is an instructor
            if (user.UserType != UserType.Instructor)
            {
                _logger.LogWarning("Instructor login failed: Non-instructor user attempted to login with email {Email}", request.Email);
                return this.Unauthorized<LoginResponseDto>("This login is for instructors only");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Instructor login failed: Invalid password for email {Email}", request.Email);
                return this.Unauthorized<LoginResponseDto>("Invalid email or password");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Instructor login failed: Deactivated account for email {Email}", request.Email);
                return this.Unauthorized<LoginResponseDto>("Account is deactivated");
            }

            var token = _jwtService.GenerateToken(user);
            var response = new LoginResponseDto
            {
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString(),
                    FullName = user.FullName,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt
                }
            };

            _logger.LogInformation("Instructor login successful for user: {UserId}", user.Id);
            return this.Success(response, "Login successful");
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] InstructorRegisterRequestDto request)
        {
            _logger.LogInformation("Instructor registration attempt for email: {Email}", request.Email);

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Instructor registration failed: Email already exists {Email}", request.Email);
                return this.BadRequest<RegisterResponseDto>("Email is already registered");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserType = UserType.Instructor, // Force instructor type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                _logger.LogWarning("Instructor registration failed for email {Email}: {Errors}", request.Email, string.Join(", ", errors));
                return this.ValidationError<RegisterResponseDto>(errors);
            }

            try
            {
                // Send welcome email
                await _emailService.SendWelcomeEmailAsync(user.Email!, user.FullName);

                // Send verification code
                await _twoFactorService.SendVerificationCodeAsync(user);

                var response = new RegisterResponseDto
                {
                    Message = "Registration successful. Please check your email for verification code.",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email!,
                        UserName = user.UserName!,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserType = user.UserType.ToString(),
                        FullName = user.FullName,
                        IsActive = user.IsActive,
                        EmailConfirmed = user.EmailConfirmed,
                        CreatedAt = user.CreatedAt
                    }
                };

                _logger.LogInformation("Instructor registration successful for user: {UserId}", user.Id);
                return this.Success(response, "Registration successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending welcome email or verification code for instructor {UserId}", user.Id);
                // Registration succeeded but email services failed
                return this.Success(new RegisterResponseDto
                {
                    Message = "Registration successful but there was an issue sending verification email. Please contact support.",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email!,
                        UserName = user.UserName!,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserType = user.UserType.ToString(),
                        FullName = user.FullName,
                        IsActive = user.IsActive,
                        EmailConfirmed = user.EmailConfirmed,
                        CreatedAt = user.CreatedAt
                    }
                }, "Registration successful with warnings");
            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<VerifyEmailResponseDto>>> VerifyEmail([FromBody] VerifyCodeRequestDto request)
        {
            _logger.LogInformation("Instructor email verification attempt for email: {Email}", request.Email);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Instructor email verification failed: User not found for email {Email}", request.Email);
                return this.BadRequest<VerifyEmailResponseDto>("User not found");
            }

            if (user.UserType != UserType.Instructor)
            {
                _logger.LogWarning("Instructor email verification failed: Non-instructor user attempted verification for email {Email}", request.Email);
                return this.BadRequest<VerifyEmailResponseDto>("This verification is for instructors only");
            }

            var isValid = await _twoFactorService.ValidateVerificationCodeAsync(user, request.Code);
            if (!isValid)
            {
                _logger.LogWarning("Instructor email verification failed: Invalid code for email {Email}", request.Email);
                return this.BadRequest<VerifyEmailResponseDto>("Invalid or expired verification code");
            }

            // Mark email as confirmed
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);

            var response = new VerifyEmailResponseDto
            {
                Message = "Email verified successfully!",
                Token = token,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    UserName = user.UserName!,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserType = user.UserType.ToString(),
                    FullName = user.FullName,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt
                }
            };

            _logger.LogInformation("Instructor email verification successful for user: {UserId}", user.Id);
            return this.Success(response, "Email verified successfully");
        }

        [HttpPost("resend-verification")]
        public async Task<ActionResult<ApiResponse<object>>> ResendVerification([FromBody] SendVerificationCodeRequestDto request)
        {
            _logger.LogInformation("Instructor resend verification attempt for email: {Email}", request.Email);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Instructor resend verification failed: User not found for email {Email}", request.Email);
                return this.BadRequest<object>("User not found");
            }

            if (user.UserType != UserType.Instructor)
            {
                _logger.LogWarning("Instructor resend verification failed: Non-instructor user attempted for email {Email}", request.Email);
                return this.BadRequest<object>("This verification is for instructors only");
            }

            if (user.EmailConfirmed)
            {
                _logger.LogWarning("Instructor resend verification failed: Email already verified for {Email}", request.Email);
                return this.BadRequest<object>("Email is already verified");
            }

            var emailSent = await _twoFactorService.SendVerificationCodeAsync(user);
            if (!emailSent)
            {
                _logger.LogError("Instructor resend verification failed: Could not send code for email {Email}", request.Email);
                return this.BadRequest<object>("Failed to send verification code. Please try again.");
            }

            _logger.LogInformation("Instructor verification code resent successfully for user: {UserId}", user.Id);
            return this.Success<object>(null!, "Verification code sent to your email");
        }

        [HttpGet("me")]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentInstructor()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("GetCurrentInstructor failed: No user ID in token");
                return this.Unauthorized<UserInfoDto>("User not authenticated");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("GetCurrentInstructor failed: User not found for ID {UserId}", userId);
                return this.NotFound<UserInfoDto>("User not found");
            }

            // Verify user is an instructor
            if (user.UserType != UserType.Instructor)
            {
                _logger.LogWarning("GetCurrentInstructor failed: Non-instructor user attempted access {UserId}", userId);
                return this.Unauthorized<UserInfoDto>("Access denied");
            }

            var response = new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType.ToString(),
                FullName = user.FullName,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedAt = user.CreatedAt
            };

            _logger.LogInformation("GetCurrentInstructor successful for user: {UserId}", user.Id);
            return this.Success(response, "User information retrieved successfully");
        }
    }
}