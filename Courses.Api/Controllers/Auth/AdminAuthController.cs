using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Courses.Domain.Identity;
using Courses.Api.Services;
using System.Security.Claims;
using Courses.Shared.DTOs.AuthDtos;
using Courses.Application.Abstraction;

namespace Courses.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/admin")]
    public class AdminAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ITwoFactorService _twoFactorService;

        public AdminAuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtService jwtService,
            IEmailService emailService,
            ITwoFactorService twoFactorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _twoFactorService = twoFactorService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Check if user is an admin
            if (user.UserType != UserType.Admin)
            {
                return Unauthorized(new { message = "This login is for administrators only" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            if (!user.IsActive)
            {
                return Unauthorized(new { message = "Account is deactivated" });
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    userType = user.UserType,
                    fullName = user.FullName
                }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminRegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email is already registered" });
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserType = UserType.Admin, // Force admin type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Registration failed", errors = result.Errors });
            }

            // Send welcome email
            await _emailService.SendWelcomeEmailAsync(user.Email!, user.FullName);

            // Send verification code
            await _twoFactorService.SendVerificationCodeAsync(user);

            return Ok(new
            {
                message = "Registration successful. Please check your email for verification code.",
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    userType = user.UserType,
                    fullName = user.FullName
                }
            });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyCodeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            if (user.UserType != UserType.Admin)
            {
                return BadRequest(new { message = "This verification is for administrators only" });
            }

            var isValid = await _twoFactorService.ValidateVerificationCodeAsync(user, request.Code);
            if (!isValid)
            {
                return BadRequest(new { message = "Invalid or expired verification code" });
            }

            // Mark email as confirmed
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                message = "Email verified successfully!",
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    userType = user.UserType,
                    fullName = user.FullName,
                    emailConfirmed = user.EmailConfirmed
                }
            });
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromBody] SendVerificationCodeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            if (user.UserType != UserType.Admin)
            {
                return BadRequest(new { message = "This verification is for administrators only" });
            }

            if (user.EmailConfirmed)
            {
                return BadRequest(new { message = "Email is already verified" });
            }

            var emailSent = await _twoFactorService.SendVerificationCodeAsync(user);
            if (!emailSent)
            {
                return BadRequest(new { message = "Failed to send verification code. Please try again." });
            }

            return Ok(new { message = "Verification code sent to your email" });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentAdmin()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Verify user is an admin
            if (user.UserType != UserType.Admin)
            {
                return Forbid();
            }

            return Ok(new
            {
                id = user.Id,
                email = user.Email,
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                userType = user.UserType,
                fullName = user.FullName,
                isActive = user.IsActive,
                emailConfirmed = user.EmailConfirmed,
                createdAt = user.CreatedAt
            });
        }
    }

}