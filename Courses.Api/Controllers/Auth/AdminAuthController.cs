using Courses.Application.Features.Authentication.Commands.Register;
using Courses.Application.Features.Authentication.Commands.ResendVerificationCode;
using Courses.Application.Features.Authentication.Commands.VerifyEmail;
using Courses.Application.Features.Authentication.Queries.GetCurrentUser;
using Courses.Application.Features.Authentication.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Courses.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/admin")]
    public class AdminAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AdminAuthController> _logger;

        public AdminAuthController(
            IMediator mediator,
            ILogger<AdminAuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var query = new LoginQuery(request, UserType.Admin);
                var result = await _mediator.Send(query);
                return this.Success(result, "Login successful");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Admin login failed: {Message}", ex.Message);
                return this.Unauthorized<LoginResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during admin login");
                return this.BadRequest<LoginResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] AdminRegisterRequestDto request)
        {
            try
            {
                var command = new AdminRegisterCommand(request);
                var result = await _mediator.Send(command);
                return this.Success(result, "Registration successful");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Admin registration failed: {Message}", ex.Message);
                return this.BadRequest<RegisterResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during admin registration");
                return this.BadRequest<RegisterResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<VerifyEmailResponseDto>>> VerifyEmail([FromBody] VerifyCodeRequestDto request)
        {
            try
            {
                var command = new VerifyEmailCommand(request, UserType.Admin);
                var result = await _mediator.Send(command);
                return this.Success(result, "Email verified successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Admin email verification failed: {Message}", ex.Message);
                return this.BadRequest<VerifyEmailResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during admin email verification");
                return this.BadRequest<VerifyEmailResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("resend-verification")]
        public async Task<ActionResult<ApiResponse<object>>> ResendVerification([FromBody] SendVerificationCodeRequestDto request)
        {
            try
            {
                var command = new ResendVerificationCodeCommand(request, UserType.Admin);
                await _mediator.Send(command);
                return this.Success<object>(null!, "Verification code sent to your email");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Admin resend verification failed: {Message}", ex.Message);
                return this.BadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during admin resend verification");
                return this.BadRequest<object>("An unexpected error occurred");
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentAdmin()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized<UserInfoDto>("User not authenticated");
                }

                var query = new GetCurrentUserQuery(userId, UserType.Admin);
                var result = await _mediator.Send(query);
                return this.Success(result, "User information retrieved successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("GetCurrentAdmin failed: {Message}", ex.Message);
                return this.BadRequest<UserInfoDto>(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning("GetCurrentAdmin failed: {Message}", ex.Message);
                return this.Unauthorized<UserInfoDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetCurrentAdmin");
                return this.BadRequest<UserInfoDto>("An unexpected error occurred");
            }
        }
    }
}