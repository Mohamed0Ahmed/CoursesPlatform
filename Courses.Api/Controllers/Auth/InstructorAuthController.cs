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
    [Route("api/auth/instructor")]
    public class InstructorAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<InstructorAuthController> _logger;

        public InstructorAuthController(
            IMediator mediator,
            ILogger<InstructorAuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            try
            {
                var query = new LoginQuery(request, UserType.Instructor);
                var result = await _mediator.Send(query);
                return this.Success(result, "Login successful");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Instructor login failed: {Message}", ex.Message);
                return this.Unauthorized<LoginResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during instructor login");
                return this.BadRequest<LoginResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<RegisterResponseDto>>> Register([FromBody] InstructorRegisterRequestDto request)
        {
            try
            {
                var command = new InstructorRegisterCommand(request);
                var result = await _mediator.Send(command);
                return this.Success(result, "Registration successful");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Instructor registration failed: {Message}", ex.Message);
                return this.BadRequest<RegisterResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during instructor registration");
                return this.BadRequest<RegisterResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("verify-email")]
        public async Task<ActionResult<ApiResponse<VerifyEmailResponseDto>>> VerifyEmail([FromBody] VerifyCodeRequestDto request)
        {
            try
            {
                var command = new VerifyEmailCommand(request, UserType.Instructor);
                var result = await _mediator.Send(command);
                return this.Success(result, "Email verified successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Instructor email verification failed: {Message}", ex.Message);
                return this.BadRequest<VerifyEmailResponseDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during instructor email verification");
                return this.BadRequest<VerifyEmailResponseDto>("An unexpected error occurred");
            }
        }

        [HttpPost("resend-verification")]
        public async Task<ActionResult<ApiResponse<object>>> ResendVerification([FromBody] SendVerificationCodeRequestDto request)
        {
            try
            {
                var command = new ResendVerificationCodeCommand(request, UserType.Instructor);
                await _mediator.Send(command);
                return this.Success<object>(null!, "Verification code sent to your email");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("Instructor resend verification failed: {Message}", ex.Message);
                return this.BadRequest<object>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during instructor resend verification");
                return this.BadRequest<object>("An unexpected error occurred");
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserInfoDto>>> GetCurrentInstructor()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return this.Unauthorized<UserInfoDto>("User not authenticated");
                }

                var query = new GetCurrentUserQuery(userId, UserType.Instructor);
                var result = await _mediator.Send(query);
                return this.Success(result, "User information retrieved successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("GetCurrentInstructor failed: {Message}", ex.Message);
                return this.BadRequest<UserInfoDto>(ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogWarning("GetCurrentInstructor failed: {Message}", ex.Message);
                return this.Unauthorized<UserInfoDto>(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during GetCurrentInstructor");
                return this.BadRequest<UserInfoDto>("An unexpected error occurred");
            }
        }
    }
}