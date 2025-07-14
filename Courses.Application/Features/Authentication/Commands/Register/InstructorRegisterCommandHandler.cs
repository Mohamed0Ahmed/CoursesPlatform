using Courses.Application.Abstraction.Email;
using Courses.Application.Abstraction.TwoFactor;
using Courses.Shared.DTOs.AuthDtos;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Courses.Application.Features.Authentication.Commands.Register;

public class InstructorRegisterCommandHandler : IRequestHandler<InstructorRegisterCommand, RegisterResponseDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ITwoFactorService _twoFactorService;
    private readonly ILogger<InstructorRegisterCommandHandler> _logger;

    public InstructorRegisterCommandHandler(
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ITwoFactorService twoFactorService,
        ILogger<InstructorRegisterCommandHandler> logger)
    {
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